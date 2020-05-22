using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_AI : MonoBehaviour
{
public Transform Motor;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;
    public float Drag = 0.1f;

    protected Rigidbody rb;
    protected Quaternion StartRotation;

    // Start is called before the first frame update
    void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.StartRotation = Motor.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var forceDirection = this.transform.forward;
        var steer = 0;

        if(Input.GetKey(KeyCode.A)){
            steer = 1;
        }
        if(Input.GetKey(KeyCode.D)){
            steer = -1;
        }

        this.rb.AddForceAtPosition(steer * this.transform.right * this.SteerPower / 100f, this.Motor.position);

        var forward = Vector3.Scale(new Vector3(1, 0, 1), this.transform.forward);
        
        if(Input.GetKey(KeyCode.W)){
            PhysicsHelper.ApplyForceToReachVelocity(this.rb, forward * this.MaxSpeed, this.Power);
        }

        if(Input.GetKey(KeyCode.S)){
            PhysicsHelper.ApplyForceToReachVelocity(this.rb, forward * -this.MaxSpeed, this.Power);
        }
    }
}
