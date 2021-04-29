using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;




public class TankCtrl : MonoBehaviour, IPunObservable
{
    private Transform tr;
    public float  speed = 10.0f;
    private PhotonView pv;

    public Transform firePos;
    public GameObject cannon;

    public Transform cannonMesh;
    public AudioClip fireSfx;
    private new AudioSource audio;

    public TMPro.TMP_Text userIdText;

    
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();


        userIdText.text = pv.Owner.NickName;

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
            
            //포탄 발사 로직
            if (Input.GetMouseButtonDown(0))
            {
               pv.RPC("Fire", RpcTarget.All,pv.Owner.NickName); //상대방의 총알이 보이게
               //RpcTarget.AllViaServer : 서버에서 동시에 뿌려주는. 그냥 All은 로컬은 바로 실행, AllBuffered(쌓아둠)
               //Fire();
            }

            //포신 회전 설정
            float r = Input.GetAxis("Mouse ScrollWheel");
            cannonMesh.Rotate(Vector3.right * Time.deltaTime * r * 1000.0f);

        }
        else
        {
            if ((tr.position - receivePos).sqrMagnitude > 3.0f * 3.0f) //임계치값을 초과하면 내 네트워크 속도가 떨어져서 패킷을 늦게 받았다고 판단해서 보간해서 움직여주는 것
            
            {
                tr.position = receivePos;
            }
            else
            {                
                tr.position = Vector3.Lerp(tr.position, receivePos, Time.deltaTime * 10.0f);
            }

            tr.rotation = Quaternion.Slerp(tr.rotation, receiveRot, Time.deltaTime * 10.0f);
        }
    }

    [PunRPC]//이게 없으면 일반함수
    
    void Fire(string shooterName)
    {
        audio?.PlayOneShot(fireSfx);
        GameObject _cannon =  Instantiate(cannon, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().shooter = shooterName;
    }

    //네트워크를 통해서 수신받을 변수
    Vector3 receivePos = Vector3.zero; 
    Quaternion receiveRot = Quaternion.identity; //

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //PhotonView.IsMine == true
        {
            stream.SendNext(tr.position); //위치
            stream.SendNext(tr.rotation); //회전
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext(); //내 탱크의 복사본인 애들이 받는
        }
    }
}
