using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks 
{
    private readonly string gameVersion = "v1.0";
    private string userId = "MH";

    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    //룸 목록 저장하기 위한 딕셔너리자료형
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    //룸을 표시할 프리팹
    public GameObject roomPrefab;
    // room 프리팹이 차일드화시킬 부모 객체
    public Transform scrollContent;


    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 자동으로 씬 로딩해주는 기능. 트루일때 로드레벨 사용가능. 방장(master)만 방 받으면 다 공유
        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        //유저명 지정
        //PhotonNetwork.NickName = userId;

        //서버접속 ping test로 가장 적합한 서버
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }

    public override void OnConnectedToMaster() // 서버에 접속
    {
        Debug.Log("Connected to Photon Server!!!");
        //PhotonNetwork.JoinRandomRoom(); //랜덤한 룸에 접속시도

        //로비에 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}");

        //룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;        

        roomNameText.text = $"ROOM _{Random.Range(0, 100):000}";

        //룸을 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }
    // 룸 생성 완료 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }
    // 룸에 입장했을 때 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField"); //이 함수를 쓰면 통신을 잠시 끊고 새 씬을 연결하고 다시 연결 재개
        }

        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        //PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0); //룸에 입장한 모든 사람의 맵에 탱크 만들어진다.
        // 0 은 그룹 아이디, 같은 그룹 아이디끼리만 보인다.

    }


    // 룸 목록이 변경(갱신) 될 때마다 호출되는 콜백함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null; //함수 안의 지역변수이기때문에 반드시 초기화가 필요하다.

        foreach (var room in roomList)
        {
            //Debug.Log($"room name= {room.Name}, ({room.PlayerCount}/{room.MaxPlayers})"); : 처음에 썼던 플레이어 카운트
            // 룸 삭제된 경우
            if (room.RemovedFromList == true)
            {
                //딕셔너리에 삭제, roomItem 프리팹 삭제
                roomDict.TryGetValue(room.Name, out tempRoom);
                //RoomItem 프리팹을 삭제
                Destroy(tempRoom);
                //딕셔너리에서 해당 데이터를 삭제
                roomDict.Remove(room.Name);
            }
            else //룸 정보가 갱신(변경)
            {
                //처음 생성된 경우 딕셔너리에 데이터 추가 + roomItem 생성
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent); //그리드 레이아웃 그룹을 넣었기 때문에 위치는 필요없다.
                    //룸 정보 표시
                    _room.GetComponent<RoomData>().RoomInfo = room; //룸 데이터의 Set부분 발생
                    //_room.GetComponentInChildren<TMP_Text>().text = room.Name; // 먼저했던거
                    //딕셔너리에 데이터 추가
                    roomDict.Add(room.Name, _room);
                }
                else
                {
                    //룸 정보를 갱신
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    //tempRoom.GetComponentInChildren<TMP_Text>().text = room.Name; //먼저헀던거
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;

                }

            }
        }
    }

    
    
#region UI_BUTTON_CALLBACK
    public void OnLoginClick()
    {
        if (string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
         //룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        if (string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";
        }


        //룸을 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);

    }

#endregion

}
