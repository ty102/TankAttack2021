using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomData : MonoBehaviour
{
    private TMP_Text roomInfoText;
    private RoomInfo _roomInfo;

    public RoomInfo RoomInfo //프로퍼티
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // room_03 (12/30)
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers}";
            //버튼의 클릭 이벤트에 함수 연결
            //룸 아이템의 버튼 아래에 온 클릭에 플러스
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener( () => OnEnterRoom(_roomInfo.Name) ); //람다식 문법으로 호출
            //GetComponent<UnityEngine.UI.Button>().onClick.AddListener( delegate () {OnEnterRoom(_roomInfo.Name);} ); 이렇게도 가능

        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
    }

    void OnEnterRoom(string roomName)
    {
        //룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }

    
}
