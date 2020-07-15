using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunk : MonoBehaviour
{
    public bool overrideRigidbody = true;
    public GameObject predecessor;
    
    public FixedJoint joint;
    public FixedJoint childJoint = null;

    protected KatinTimer despawnTimer;
    public float despawnTime;

    
    void Start() {
        this.despawnTimer = new KatinTimer();
    }

    void Update()
    {
        this.despawnTimer.update();        
    }

    void OnJointBreak(float breakForce){
        Rigidbody hunkBody = this.gameObject.GetComponent<Rigidbody>();
        if(hunkBody != null){
            hunkBody.useGravity = true;
        }

        if(this.childJoint != null){
            this.childJoint.breakForce = 0;
        }

        this.despawnTimer.Init(this.despawnTime, this.despawn);
    }

    public void despawn(){
        this.gameObject.SetActive(false);
    }

}
