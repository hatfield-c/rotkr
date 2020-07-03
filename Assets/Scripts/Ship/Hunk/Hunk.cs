using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunk : MonoBehaviour
{
    public bool overrideRigidbody = true;
    public GameObject predecessor;
    
    public FixedJoint joint;
    public FixedJoint childJoint = null;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJointBreak(float breakForce){
        Rigidbody hunkBody = this.gameObject.GetComponent<Rigidbody>();
        if(hunkBody != null){
            hunkBody.useGravity = true;
        }

        if(this.childJoint != null){
            this.childJoint.breakForce = 0;
        }
    }

}
