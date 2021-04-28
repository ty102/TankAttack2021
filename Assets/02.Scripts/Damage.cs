using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    public int hp = 100;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponentsInChildren<MeshRenderer>(renderers);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("CANNON"))
        {
            string shooter = coll.gameObject.GetComponent<Cannon>().shooter;
            hp -= 10;
            if (hp <= 0)
            {
               StartCoroutine(TankDestroy(shooter)); 
               //StartCoroutine("TankDestroy", shooter); 개별 정지 불가능하기때문에 가급적 사용 X. 
            }

        }
    }

    IEnumerator TankDestroy(string shooter)
    {
        string msg = $"\n<color=#00ff00>{pv.Owner.NickName}</color> is killed by <color=#ff0000>{shooter}</color>";
        GameManager.instance.messageText.text += msg;

        // 발사로직을 정지
        // 렌더러 컴포넌트를 비활성화
        GetComponent<BoxCollider>().enabled = false; //박스 비활성
        if (pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = true; // 비활성, 자기자신일때만이라서 false 아니고 pv. IsMine
        }
        foreach(var mesh in renderers) mesh.enabled = false;

        // 5초동안 대기
        yield return new WaitForSeconds(5.0f);

        // 불규칙한 위치로 이동
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));

        transform.position = pos;

        // 렌더러 컴포넌트를 활성화
        hp = 100;
        GetComponent<BoxCollider>().enabled = true;
        if(pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = false; // 활성
        }
        foreach(var mesh in renderers) mesh.enabled = true;


    }
}
