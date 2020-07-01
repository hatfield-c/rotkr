﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyManager : MonoBehaviour {
    #region proposed changes for rat health
    public Action UnderWater;
    #endregion


    public Transform waterLevel;
    public Transform[] floatPoints;
    public FloatDeadzone deadzone;
    public BuoyancyParameters parameters;

    protected Rigidbody Rigidbody;
    protected float hammerVelocity;

    void Start(){
        this.Rigidbody = GetComponent<Rigidbody>();

        float buoyancyAccel = this.parameters.force / this.Rigidbody.mass;
        this.hammerVelocity = Mathf.Sqrt(2 * this.parameters.maxDepth * buoyancyAccel);
    }

    void FixedUpdate(){
        float equil = this.waterLevel.position.y;

        List<int> abovewaterPoints = this.aboveWaterIndexes(floatPoints, equil);
        List<int> underwaterPoints = this.underWaterIndexes(floatPoints, equil);
        List<int> deadPoints = this.deadIndexes(floatPoints, equil);

        if(abovewaterPoints.Count < 1){
            this.Rigidbody.useGravity = false;
            UnderWater?.Invoke();
        } else {
            this.Rigidbody.useGravity = true;
        }

        if (underwaterPoints.Count == 0){
            return;
        }

        if(
            this.Rigidbody.velocity.y < 0 && 
            this.transform.position.y < equil - this.deadzone.min - this.parameters.maxDepth &&
            this.Rigidbody.velocity.y < -this.hammerVelocity
        ){
            this.Rigidbody.AddForce(
                this.Rigidbody.mass * (-this.Rigidbody.velocity.y - this.hammerVelocity) * Vector3.up, 
                ForceMode.Impulse
            );
        }

        foreach(int i in underwaterPoints){
            float depth = equil - this.floatPoints[i].position.y;
            float force = this.parameters.buoyantForce(depth);

            this.Rigidbody.AddForceAtPosition(force * Vector3.up, this.floatPoints[i].position);
        }

        if(deadPoints.Count == (int)((3 / 4) * this.floatPoints.Length)){
            this.Rigidbody.angularVelocity = new Vector3(
                this.Rigidbody.angularVelocity.x * this.parameters.torqueDamping,
                this.Rigidbody.angularVelocity.y,
                this.Rigidbody.angularVelocity.z * this.parameters.torqueDamping
            );
        }
    }

    public List<int> aboveWaterIndexes(Transform[] floatPoints, float targetY){
        List<int> abovewaterPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(floatPoints[i].position.y > targetY + this.deadzone.max){
                abovewaterPoints.Add(i);
            }
        }

        return abovewaterPoints;
    }

    public List<int> underWaterIndexes(Transform[] floatPoints, float targetY){
        List<int> underwaterPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(floatPoints[i].position.y < targetY + this.deadzone.min){
                underwaterPoints.Add(i);
            }
        }

        return underwaterPoints;
    }

    public List<int> deadIndexes(Transform[] floatPoints, float targetY){
        List<int> deadPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(
                floatPoints[i].position.y > targetY + this.deadzone.min &&
                floatPoints[i].position.y < targetY + this.deadzone.max
            ){
                deadPoints.Add(i);
            }
        }

        return deadPoints;
    }
}
