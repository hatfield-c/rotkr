using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantForce
{
    public Vector3 position;
    public Vector3 force;

    public BuoyantForce(){
        this.position = new Vector3(0f, 0f, 0f);
        this.force = new Vector3(0f, 0f, 0f);
    }

    public void setPosition(float x, float y, float z){
        this.position.x = x;
        this.position.y = y;
        this.position.z = z;
    }

    public void setForce(float force){
        this.force.y = force;
    }

    public void Null(){
        this.setPosition(0f, 0f, 0f);
        this.setForce(0f);
    }
}
