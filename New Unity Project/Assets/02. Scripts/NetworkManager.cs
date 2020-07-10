using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//네트워크 메니저 : 룸(게임공간0으로 이동해줌
//포톤네트워크 : 마스터서버>로비(대기실)>룸(게임공간)
// MonoBehaviourPunCallbacks : 포톤 서버 접속, 로비 접속, 룸 접속시 신호 받아옴

//포톤서버 접속,로비접속, 룸 접속 등 이벤트를 받아올 수 있음
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text infoText;
    public Button connect;

    string gameVersion = "1";       //게임버전

    private void Awake()
    {
        //해상도조절
        Screen.SetResolution(800, 600, FullScreenMode.Windowed);
    }

    //네트워트 매니저 실행시 가장 먼저 실행

    // Start is called before the first frame update
    void Start()
    {
        //게임버전업데이트
        PhotonNetwork.GameVersion = gameVersion;        //게임버전(Poton.Pun)을 선언해주지 않으면 실행되지 않음

        //마스터 서버 접속
        PhotonNetwork.ConnectUsingSettings();

        //접촉시도중임을 텍스트로 표시하기
        infoText.text = "마스터 서버에 접속중...";

        //룸 접속 버튼 비활성화
        connect.interactable = false;
        
    }

    public override void OnConnectedToMaster()
    {
        infoText.text = "온라인 : 마스터 서버와 연결됨";
        connect.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 접속 버튼 비활성화
        connect.interactable = false;
        //접속 정보 표시
        infoText.text = "오프라인 : 마스터 서버와 연결실패 \n 접속 재시도 중...";
        PhotonNetwork.ConnectUsingSettings();//마스터 서버 접속 함수
    }

    public void OnConnect()
    {
        connect.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            infoText.text = "랜덤방에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            infoText.text = "오프라인 : 마스터 서버와 연결실패 \n 접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();//마스터 서버 접속 함수
        }
    }

    public override void OnJoinedRoom()
    {
        infoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "빈 방이 없으니 새로운 방 생성중...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
