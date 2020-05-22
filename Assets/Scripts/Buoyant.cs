using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyant : MonoBehaviour
{
    
    public GameObject equil;
    public float threshold = 0.01F;
    public float buoyForce = 10F;

    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float equilPoint = equil.transform.position.y + threshold;
        float selfY = transform.position.y;
        float diff = selfY - equilPoint;

        //Vector3 bouyantForce = 

        if(selfY < equilPoint){
            //Debug.Log("flag");
            //Debug.Log(selfY);
            //Debug.Log(equilPoint);
            this.rb.velocity = Vector3.zero;
            this.rb.AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
    }
}
