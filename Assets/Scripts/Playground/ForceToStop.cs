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
        float velDesired = 5f;

        if(this.rb.velocity.z > velDesired){
            Debug.Log(this.vel);
            this.vel = this.rb.velocity;
            Debug.Log("flag");

            this.rb.AddForce(new Vector3(0, 0, -5), ForceMode.Impulse);

            this.timer += Time.deltaTime;
        } else {
            Debug.Log(this.rb.velocity.z);
            Debug.Log(this.timer);
        }
    }


}
