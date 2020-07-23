using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTarget : MonoBehaviour {
    
    public bool isHit = false;

    void OnTriggerEnter(Collider other){
        this.isHit = true;
    }

    public void resetSelf(){
        this.isHit = false;
    }
}
