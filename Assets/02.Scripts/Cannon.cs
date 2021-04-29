using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float speed = 2000.0f;
    public GameObject exp;
    public string shooter;

    
    

    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject obj = Instantiate(exp, transform.position, Quaternion.identity);
        Destroy(obj, 3.0f);
        Destroy(this.gameObject);
    }

    



    

}
