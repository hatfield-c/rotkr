using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatToShip : MonoBehaviour
{
    public Transform ship;
    public Rigidbody ratbody;
    public float breakForce = 10f;

    void Start()
    {
        this.ratbody = this.gameObject.GetComponent<Rigidbody>();
        this.ratbody.isKinematic = true;
        this.ratbody.useGravity = false;

        this.transform.parent = this.ship;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        Debug.Log($"flag: {force}");

        if(force >= this.breakForce){
            this.transform.parent = null;
            this.ratbody.isKinematic = false;
            this.ratbody.useGravity = true;
        }
    }
}
