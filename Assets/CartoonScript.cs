using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonScript : MonoBehaviour
{
    public float timer = 3.8f;
    public float timerValue = 3.8f;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerValue;
        this.gameObject.SetActive(true);
        
        this.gameObject.SetActive(false);
    }
    public void activate()
    {
        Debug.Log("Activate from Cartoon");
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Cartoon update");
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Debug.Log("Cartoon countDown");
            this.gameObject.SetActive(false);
        }
    }
}
