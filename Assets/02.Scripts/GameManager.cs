using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        //PhotonNetwork.IsMessageQueueRunning = true;

        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));
        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0); //룸에 입장한 모든 사람의 맵에 탱크 만들어진다.
        // 0 은 그룹 아이디, 같은 그룹 아이디끼리만 보인다.
    }
}
