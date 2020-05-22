using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blok : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJointBreak(float breakForce)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
