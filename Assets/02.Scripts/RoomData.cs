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
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
    }

    
}
