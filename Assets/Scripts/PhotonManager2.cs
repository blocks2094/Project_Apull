using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonManager2 : MonoBehaviourPunCallbacks
{
    // 게임의 버전
    private readonly string version = "1.0";
    // 유저의 닉네임
    private string userId = "Zack";
    void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;
        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        // 접속 유저의 닉네임 설정
        PhotonNetwork.NickName = userId;
        // 포톤 서버와의 데이터의 초당 전송 횟수
        Debug.Log(PhotonNetwork.SendRate);
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }
    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode}:{message}");
        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸 목록에 노출시킬 여부
                             // 룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }
        // 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        // 네트워크상에 캐릭터 생성
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}