using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public void Impulse()
    {
        // set random rotation
        this.transform.Rotate(Random.Range(45, 300f), Random.Range(45, 300f), Random.Range(45, 300f));
        // add force
        GetComponent<Rigidbody>().AddForce(new Vector3(0.4f, -0.2f, -0.4f), ForceMode.Impulse);
        // add rotation
        GetComponent<Rigidbody>().AddTorque(new Vector3(180f, 180f));
    }

    public byte Value()
    {
        if (Vector3.Dot(Vector3.up, transform.up) > 0.9)
        {
            return 2;
        }
        else if (Vector3.Dot(Vector3.up, -transform.up) > 0.9)
        {
            return 5;
        }
        else if (Vector3.Dot(Vector3.up, transform.right) > 0.9)
        {
            return 6;
        }
        else if (Vector3.Dot(Vector3.up, -transform.right) > 0.9)
        {
            return 1;
        }
        else if (Vector3.Dot(Vector3.up, transform.forward) > 0.9)
        {
            return 3;
        }
        return 4;
    }
}
