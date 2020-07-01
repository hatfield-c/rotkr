using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceToStop : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 vel;
    
    protected float timer = 0f;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.rb.velocity = new Vector3(0, 0, 10);
        this.vel = this.rb.velocity;

        //this.rb.AddForce(new Vector3(0, 0, -10f), ForceMode.Force);
    }

    void FixedUpdate()
    {
        if(this.rb.velocity.z > 0){
            Debug.Log(this.vel);
            this.vel = this.rb.velocity;
            this.rb.AddForce(new Vector3(0, 0, -2.5f), ForceMode.Force);
            this.timer += Time.deltaTime;
        } else {
            Debug.Log(this.timer);
        }
    }


}
