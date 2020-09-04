using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    protected bool collideCheck = false;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision A");
        if (this.collideCheck){
            return;
        }

        this.collideCheck = true;
        Debug.Log("Collision B");
        this.transform.position = new Vector3(0, 2, -10);

    }

    void FixedUpdate()
    {
        this.collideCheck = false;
    }
}
