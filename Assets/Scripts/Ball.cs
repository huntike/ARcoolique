using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isThrow = false;
    public Vector3 forceLancer = new Vector3(0f,4.5f,1f);
    public Vector3 forceLancer2 = new Vector3(0f,1f,1f);

    public BallThrower parrent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.transform.position.y < -20 || gameObject.transform.position.y > 40 || gameObject.transform.position.z > 20)
        {
            ResetPos();
        }
        
    }

    public void ResetPos(){
            // gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            // gameObject.transform.position = Vector3.zero;
            gameObject.transform.position = new Vector3(0, -0.45f, 3.15f);

            isThrow = false;
            parrent.isThrow= false;
    }
}
