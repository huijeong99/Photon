using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRotate : MonoBehaviourPun
{
    //카메라를 마우스움직이는 방향으로 회전하기
    public float speed = 150;
    //회전각도를 직접 제어하자
    float angleX;

    //카메라
    Transform cam;

    private void Start()
    {
        if (photonView.IsMine) return;//내 카메라는 제거하지 않는다

        //내가 아니면 상대방의 형태나 카메라 태그를 변경
        //상대방의 카메라와 오디오를 비활성
        cam = transform.Find("Main Camera");
        cam.tag = "Untagged";
        cam.GetComponent<Camera>().enabled = false;
        cam.GetComponent<AudioListener>().enabled = false;
        cam.parent.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float h = Input.GetAxis("Mouse X");
        angleX += h * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, angleX, 0);
    }
}
