using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;


public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    public float  speed = 10.0f;
    private PhotonView pv;

    
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform; //그냥게임오브젝트파인드와 다르게 tr아래에서부터뒤짐.
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }

    
    void Update()
    {
        if (pv.IsMine) //내 탱크일때만 조종
        {

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            tr.Translate (Vector3.forward* Time.deltaTime * speed * v);
            tr.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);

        }
    }
}
