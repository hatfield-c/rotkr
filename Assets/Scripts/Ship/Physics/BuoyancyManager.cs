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

    public WaterCalculator waterLevel;
    public Transform[] floatPoints;
    public FloatDeadzone floatzone;
    public BuoyancyParameters parameters;
    public WaterFrictionParameters friction;

    protected Rigidbody Rigidbody;
    protected float hammerVelocity;

    private Vector3 frictionForceBuffer;

    void Start(){
        this.Rigidbody = this.GetComponent<Rigidbody>();

        float buoyancyAccel = this.parameters.force / this.Rigidbody.mass;
        this.hammerVelocity = Mathf.Sqrt(2 * this.parameters.maxDepth * buoyancyAccel);
    }

    void FixedUpdate(){
        // TODO: PERHAPS REFACTOR THIS LATER??? Dependency injection riddle with this.waterLevel
        if (waterLevel == null)
            return;

        float targetCenter = this.waterLevel.calculateHeight(
            this.transform.TransformPoint(this.Rigidbody.centerOfMass).x,
            this.transform.TransformPoint(this.Rigidbody.centerOfMass).z
        );
        float[] targets = this.getFloatTargets(this.floatPoints);

        List<int> abovewaterPoints = this.aboveWaterIndexes(this.floatPoints, targets);
        List<int> underwaterPoints = this.underWaterIndexes(this.floatPoints, targets);
        List<int> stablePoints = this.stableIndexes(this.floatPoints, targets);

        if(abovewaterPoints.Count < 1){
            this.Rigidbody.useGravity = false;
            UnderWater?.Invoke();
        } else {
            this.Rigidbody.useGravity = true;
        }

        if(stablePoints.Count < 1 && underwaterPoints.Count < 1){
            return;
        }

        this.applyHammerForce(targetCenter);

        foreach(int i in underwaterPoints){
            float depth = targets[i] - this.floatPoints[i].position.y;
            this.applyBuoyancy(this.floatPoints[i], depth);    
        }


        this.applyStableForce(targetCenter);
        this.dampenBobbing(stablePoints);
        this.applyWaterFriction();
    }
    public void Init(GameObject waterPlane) {
        this.waterLevel = waterPlane.GetComponent<WaterCalculator>();
    }

    protected void applyBuoyancy(Transform floatPoint, float depth){
        if(this.Rigidbody.velocity.y > this.parameters.maxVelocity){
            return;
        }

        float force = this.parameters.buoyantForce(depth);

        if(force > this.parameters.maxForce){
            force = this.parameters.maxForce;
        }

        this.Rigidbody.AddForceAtPosition(force * Vector3.up, floatPoint.position);
    }

    protected void applyHammerForce(float targetCenter){
        if(
            this.Rigidbody.velocity.y < 0 && 
            this.transform.position.y < targetCenter + this.floatzone.floatMin - this.parameters.maxDepth &&
            this.Rigidbody.velocity.y < -this.hammerVelocity
        ){
            this.Rigidbody.AddForce(
                this.Rigidbody.mass * (-this.Rigidbody.velocity.y - this.hammerVelocity) * Vector3.up, 
                ForceMode.Impulse
            );
        }
    }

    protected void applyStableForce(float targetCenter){

        float stableForce = this.floatzone.stableForce(
            this.Rigidbody.velocity.y, 
            this.transform.TransformPoint(this.Rigidbody.centerOfMass).y,
            targetCenter
        );
        this.Rigidbody.AddForce(stableForce * Vector3.up, ForceMode.Acceleration);
    }

    protected void applyWaterFriction(){
        float frictionForce = this.friction.frictionForce(
            this.Rigidbody.velocity.magnitude
        );

        this.frictionForceBuffer.x = -this.Rigidbody.velocity.normalized.x * frictionForce;
        this.frictionForceBuffer.y = 0;
        this.frictionForceBuffer.z = -this.Rigidbody.velocity.normalized.z * frictionForce;
        
        this.Rigidbody.AddForce(this.frictionForceBuffer);
    }

    protected void dampenBobbing(List<int> stablePoints){
        if(stablePoints.Count == (int)((3 / 4) * this.floatPoints.Length)){
            this.Rigidbody.angularVelocity = new Vector3(
                this.Rigidbody.angularVelocity.x * this.parameters.torqueDamping,
                this.Rigidbody.angularVelocity.y,
                this.Rigidbody.angularVelocity.z * this.parameters.torqueDamping
            );
        }
    }

    protected float[] getFloatTargets(Transform[] floatPoints){
        float[] targets = new float[floatPoints.Length];

        for(int i = 0; i < floatPoints.Length; i++){
            targets[i] = this.waterLevel.calculateHeight(
                floatPoints[i].position.x,
                floatPoints[i].position.z
            );
        }

        return targets;
    }

    protected List<int> aboveWaterIndexes(Transform[] floatPoints, float[] targets){
        List<int> abovewaterPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(this.floatzone.isAboveWater(floatPoints[i].position.y, targets[i])){
                abovewaterPoints.Add(i);
            }
        }

        return abovewaterPoints;
    }

    protected List<int> underWaterIndexes(Transform[] floatPoints, float[] targets){
        List<int> underwaterPoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(this.floatzone.isUnderWater(floatPoints[i].position.y, targets[i])){
                underwaterPoints.Add(i);
            }
        }

        return underwaterPoints;
    }

    protected List<int> stableIndexes(Transform[] floatPoints, float[] targets){
        List<int> stablePoints = new List<int>();

        for(int i = 0; i < floatPoints.Length; i++){
            if(this.floatzone.isInFloatZone(floatPoints[i].position.y, targets[i])){
                stablePoints.Add(i);
            }
        }

        return stablePoints;
    }

    protected String floatPointsDepths(){
        String str = "";
        for(int i = 0; i < this.floatPoints.Length; i++){
            str += this.floatPoints[i].position.y.ToString() + ", ";
        }

        return str;
    }
}
