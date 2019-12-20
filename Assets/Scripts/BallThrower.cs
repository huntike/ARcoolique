using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{

    public GameObject ball;
    public GameObject ARCamera;
    private Vector3 position;
    private float width;
    private float height;
    public bool isThrow = false;
    public Vector3 throwForce = new Vector3(0f,1f,1f);
    private Vector2 touchBegan;
    private Vector2 touchEnd;
    public menu menu;


    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // void OnGUI()
    // {
    //     // Compute a fontSize based on the size of the screen width.
    //     GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

    //     GUI.Label(new Rect(10, 200, Screen.width-20, height * 0.25f),
    //         // "x = " + position.x.ToString("f2") +
    //         "   throwForce = " + throwForce.ToString("f2")+
    //         ", camera Rotation = " + ARCamera.transform.rotation.ToString("f2"));
    //         // ", ballY = " + ball.GetComponent<Transform>().position.y);
    // }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(isThrow)
        // {
        //     ThrowBall();
        // }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if(!isThrow)
            {
                ThrowBall();
            }
        }
        Debug.Log("touchCount");
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Debug.Log("touchCount");
            Touch touch = Input.GetTouch(0);

            // if (touch.phase == TouchPhase.Moved)
            // {
            //     Vector2 pos = touch.position;
            //     pos.x = (pos.x - width) / width;
            //     pos.y = (pos.y - height) / height;
            //     position = new Vector3(-pos.x, pos.y, 0.0f);
            //     // if(!isThrow)
            //     // {
            //     //     ThrowBall();
            //     // }

            //     // // Position the cube.
            //     // ball.transform.position = position;
            // }

            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    Vector2 pos = touch.position;
                    pos.x = (pos.x - width) / width;
                    pos.y = (pos.y - height) / height;
                    touchBegan = new Vector3(-pos.x, pos.y, 0.0f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Halve the size of the cube.
                    Vector2 pos = touch.position;
                    pos.x = (pos.x - width) / width;
                    pos.y = (pos.y - height) / height;
                    touchEnd = new Vector3(-pos.x, pos.y, 0.0f);
                    
                    float deltaTouchX = touchEnd.x - touchBegan.x;
                    float deltaTouchY = touchEnd.y - touchBegan.y;

                    Debug.Log("deltaTouchY : " + deltaTouchY);
                    Debug.Log("deltaTouchY*11 : " + deltaTouchY*11);
                    Debug.Log("-deltaTouchY*100 : " + deltaTouchY*110);
                    // Debug.Log("deltaTouchY/10 : " + deltaTouchY/10);
                    // Debug.Log("deltaTouchY/11 : " + deltaTouchY/11);
                    // Debug.Log("-------------------------");
                    Debug.Log("deltaTouchX : " + deltaTouchX);
                    Debug.Log("deltaTouchX*10 : " + deltaTouchX*10);
                    // Debug.Log("-deltaTouchX/9 : " + deltaTouchX/9);
                    // Debug.Log("deltaTouchX/10 : " + deltaTouchX/10);
                    // Debug.Log("deltaTouchX/11 : " + deltaTouchX/11);
                    Debug.Log("deltaTouch---------------------");
                    
                    throwForce.z = Mathf.Abs(deltaTouchY*110);
                    throwForce.x = -deltaTouchX*10;

                    if(!isThrow)
                    {
                        ThrowBall();
                        touchBegan = Vector2.zero;
                        touchEnd = Vector2.zero; 
                    }

                }
            }

            if (Input.touchCount == 3)
            {
                this.menu.sceneLoader();
            }
        }
    }

    void ThrowBall()
    {
        Debug.Log("prepare to set to true");
        isThrow = true;
        ball.GetComponent<Ball>().isThrow = true;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(throwForce);
        Debug.Log("well set to true");
    }
}
