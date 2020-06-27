//using Ditzelgames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    public Transform waterLevel;
    public Transform[] FloatPoints;
    public FloatDeadzone deadzone;
    public BuoyancyParameters buoyancyParameters;
 
    protected Rigidbody Rigidbody;
    protected BuoyancyManager buoyManager;
    protected BuoyantForce bf;

    void Awake() {
        this.Rigidbody = GetComponent<Rigidbody>();
    }

    void Start(){
        this.buoyManager = new BuoyancyManager(
            this.Rigidbody, 
            this.waterLevel, 
            this.deadzone, 
            this.buoyancyParameters
        );
    }

    void FixedUpdate(){

        this.bf = this.buoyManager.update(this.FloatPoints);

        this.Rigidbody.AddForceAtPosition(bf.force, bf.position);

    }
}