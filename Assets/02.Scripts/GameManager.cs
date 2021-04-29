using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public TMP_Text messageText;

    [Header("Chatting UI")]
    public TMP_Text chatListText;
    public TMP_InputField msgIF;

    public Button exitButton;

    private PhotonView pv;


    public static GameManager instance = null; //싱글턴변수

    void Awake()
    {
        instance = this;

        //PhotonNetwork.IsMessageQueueRunning = true;

        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));
        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0); //룸에 입장한 모든 사람의 맵에 탱크 만들어진다.
        // 0 은 그룹 아이디, 같은 그룹 아이디끼리만 보인다.
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        SetRoomInfo();
    }

    void SetRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom; //using Photon.Realtime;필요
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
    }

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    //CleanUp 끝난 후에 호출되는 콜백
    public override void OnLeftRoom()
    {
        //Lobby 씬으로 되돌아가기, using UnityEngine.SceneManagement;
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       SetRoomInfo();
       string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
       messageText.text += msg;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        messageText.text += msg;
    }

    public void OnSendClick() //버튼클릭시호출할함수
    {
        string _msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color>{msgIF.text}";
        pv.RPC("SendChatMessage", RpcTarget.AllBufferedViaServer, _msg); //allbufferedviaserver 자기자신은 물론 나중에 입장한 사람도 메시지 볼 수 있게.
    }

    [PunRPC]
    void SendChatMessage(string msg)
    {
        chatListText.text += $"{msg}\n";
    }
}
