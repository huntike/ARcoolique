using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUPDetection : MonoBehaviour
{
    public GameObject Animation;

     void OnCollisionEnter(Collision col){
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
