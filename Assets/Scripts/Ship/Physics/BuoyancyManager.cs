using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyManager : MonoBehaviour {
    public Transform waterLevel;
    public Transform[] floatPoints;
    public FloatDeadzone deadzone;
    public BuoyancyParameters parameters;

    protected Rigidbody Rigidbody;

    void Start(){
        this.Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        float equil = this.waterLevel.position.y;

        List<int> abovewaterPoints = this.aboveWaterIndexes(floatPoints, equil);
        List<int> underwaterPoints = this.underWaterIndexes(floatPoints, equil);

        if(abovewaterPoints.Count < 1){
            this.Rigidbody.useGravity = false;
        } else {
            this.Rigidbody.useGravity = true;
        }

        if(underwaterPoints.Count == 0){
            return;
        }

        foreach(int i in underwaterPoints){
            float depth = equil - this.floatPoints[i].position.y;
            float force = this.parameters.calculate(depth);

            this.Rigidbody.AddForceAtPosition(force * Vector3.up, this.floatPoints[i].position);
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
}
