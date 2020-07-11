using System;
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
    public FloatDeadzone floatzone;
    public BuoyancyParameters parameters;
    public WaterFrictionParameters friction;

    protected Rigidbody Rigidbody;
    protected float hammerVelocity;

    private Vector3 frictionForceBuffer;

    void Start(){
        this.floatzone.Init(this.waterLevel);
        this.Rigidbody = this.GetComponent<Rigidbody>();

        float buoyancyAccel = this.parameters.force / this.Rigidbody.mass;
        this.hammerVelocity = Mathf.Sqrt(2 * this.parameters.maxDepth * buoyancyAccel);
    }

    void FixedUpdate(){
        float equil = this.waterLevel.position.y;

        List<int> abovewaterPoints = this.aboveWaterIndexes(floatPoints, equil);
        List<int> underwaterPoints = this.underWaterIndexes(floatPoints, equil);
        List<int> stablePoints = this.stableIndexes(floatPoints, equil);

        if(abovewaterPoints.Count < 1){
            this.Rigidbody.useGravity = false;
            UnderWater?.Invoke();
        } else {
            this.Rigidbody.useGravity = true;
        }

        if(stablePoints.Count < 1 && underwaterPoints.Count < 1){
            return;
        }

        this.applyHammerForce(equil);

        foreach(int i in underwaterPoints){
            float depth = equil - this.floatPoints[i].position.y;
            this.applyBuoyancy(this.floatPoints[i], depth);    
        }


        this.applyStableForce();
        this.dampenBobbing(stablePoints);
        this.applyWaterFriction();
    }

    public void applyBuoyancy(Transform floatPoint, float depth){
        float force = this.parameters.buoyantForce(depth);

        this.Rigidbody.AddForceAtPosition(force * Vector3.up, floatPoint.position);
    }

    public void applyHammerForce(float equil){
        if(
            this.Rigidbody.velocity.y < 0 && 
            this.transform.position.y < equil + this.floatzone.floatMin - this.parameters.maxDepth &&
            this.Rigidbody.velocity.y < -this.hammerVelocity
        ){
            this.Rigidbody.AddForce(
                this.Rigidbody.mass * (-this.Rigidbody.velocity.y - this.hammerVelocity) * Vector3.up, 
                ForceMode.Impulse
            );
        }
    }

    public void applyStableForce(){
        float stableForce = this.floatzone.stableForce(
            this.Rigidbody.velocity.y, 
            this.transform.TransformPoint(this.Rigidbody.centerOfMass).y
        );
        this.Rigidbody.AddForce(stableForce * Vector3.up, ForceMode.Acceleration);
    }

    public void applyWaterFriction(){
        float frictionForce = this.friction.frictionForce(
            this.Rigidbody.velocity.magnitude
        );

        this.frictionForceBuffer.x = -this.Rigidbody.velocity.normalized.x * frictionForce;
        this.frictionForceBuffer.y = 0;
        this.frictionForceBuffer.z = -this.Rigidbody.velocity.normalized.z * frictionForce;
        
        this.Rigidbody.AddForce(this.frictionForceBuffer);
    }

    public void dampenBobbing(List<int> stablePoints){
        if(stablePoints.Count == (int)((3 / 4) * this.floatPoints.Length)){
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
            if(this.floatzone.isAboveWater(floatPoints[i].position.y)){
                abovewaterPoints.Add(i);
            }
        }

        return abovewaterPoints;
    }

    public List<int> underWaterIndexes(Transform[] floatPoints, float targetY){
        List<int> underwaterPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(this.floatzone.isUnderWater(floatPoints[i].position.y)){
                underwaterPoints.Add(i);
            }
        }

        return underwaterPoints;
    }

    public List<int> stableIndexes(Transform[] floatPoints, float targetY){
        List<int> stablePoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(this.floatzone.isInFloatZone(floatPoints[i].position.y)){
                stablePoints.Add(i);
            }
        }

        return stablePoints;
    }

    public String floatPointsDepths(){
        String str = "";
        for(int i = 0; i < this.floatPoints.Length; i++){
            str += this.floatPoints[i].position.y.ToString() + ", ";
        }

        return str;
    }
}
