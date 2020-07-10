using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{

    public float _hp = 1;
    public float hp
    {
        get
        {
            return _hp;
        }

        set
        {
            hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
                print("GameOver");

                if (photonView.IsMine)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient);

                    }
                    PhotonNetwork.LoadLevel("LobbyScene");
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }

   // value= void setHP(float hp) { _hp = hp; }

    public GameObject bulletImpactFactory;  //총알파편 프리팹

    void Update()
    {
        //마우스왼쪽버튼 클릭시 레이캐스트로 총알발사
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            
            //레이랑 충돌했냐?
            if (Physics.Raycast(ray, out hitInfo))
            {
                print("충돌오브젝트 : " + hitInfo.collider.name);

                //c충돌 이펙트 보여주기(모든 사용자들에게
                photonView.RPC("ShowEffect", RpcTarget.All, hitInfo.point, hitInfo.normal);

                PhotonView enemy = hitInfo.transform.GetComponent<PhotonView>();
                if (enemy)
                {
                    enemy.RPC("Damage", RpcTarget.All, 1.0f);
                }
            }
        }
    }

    [PunRPC]
    void ShowEffect(Vector3 pos, Vector3 normal)
    {
        //충돌지점에 총알이펙트 생성한다
        GameObject bulletImpact = Instantiate(bulletImpactFactory);
        //부딪힌 지점 hitInfo안에 정보들이 담겨 있다
        bulletImpact.transform.position = pos;
        //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다
        bulletImpact.transform.forward = normal;
    }

    [PunRPC]
    void Damage(float value)
    {
        hp -= value;
    }
}
