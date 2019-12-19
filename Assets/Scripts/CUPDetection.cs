using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUPDetection : MonoBehaviour
{
    public GameObject triger;
    public GameObject Animation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
     void OnCollisionEnter( Collision col){
         if (col.gameObject.name == "_ball")
         {
            Debug.Log("OncolideWith Ball");
            Animation.GetComponent<CartoonScript>().activate();
            Animation.GetComponent<CartoonScript>().activate();
            col.gameObject.GetComponent<Ball>().ResetPos();
            gameObject.transform.parent.gameObject.SetActive(false);
         }
     }
}
