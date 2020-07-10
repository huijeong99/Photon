using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{
    //플레이어 스폰 위치
    public Transform[] spownPoints;

    private void Awake()
    {
        Screen.SetResolution(800, 500, FullScreenMode.Windowed);
    }

    //게임 매니저 역할
    //사용자가 게임세상에 들어올 경우 플레이어 하나를 생성
    //프리팹으로 플레이어가 만들어져 있을 경우 프리팹은 반드시 Resource경로 안에 들어가야 한다

    // Start is called before the first frame update
    void Start()
    {
        //전송속도 관련 세팅
        //네트워크 프레임 설정(절대적이지는 않음)
        //RPC전송률과 관련됨
        PhotonNetwork.SendRate = 30;
        //SocketSend,Receive
        PhotonNetwork.SerializationRate = 30;


        CreatePlayer();
    }

    private void CreatePlayer()
    {
        //포톤 네트워크를 사용해 생성
        int index = UnityEngine.Random.Range(0, spownPoints.Length);
        PhotonNetwork.Instantiate("Player", spownPoints[index].position,spownPoints[index].rotation);
    }
}
