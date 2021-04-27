using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret : MonoBehaviour
{
    private PhotonView pv; 

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        this.enabled = pv.IsMine; //내 것 아니면 이 스크립트는 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        float r = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * Time.deltaTime * 200.0f * r);
    }
}
