using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Move_Target : MonoBehaviour {
    
    public bool isHit = false;

    Transform trans;

    void Start(){
        this.trans = this.GetComponent<Transform>();
    }

    void OnTriggerStay(Collider other){
        this.isHit = true;
    }

    void OnTriggerExit(Collider other){
        this.isHit = false;
    }

    public void resetSelf(){
        this.isHit = false;
    }

    public Transform getTransform(){
        return this.GetComponent<Transform>();//this.transform;
    }
}
