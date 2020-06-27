using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyManager {
    protected Rigidbody shipBody;
    protected Transform waterLine;
    protected FloatDeadzone deadzone;
    protected BuoyancyParameters parameters; 
    protected BuoyantForce buoyantForce;

    public BuoyancyManager(
        Rigidbody shipBody, 
        Transform waterLine, 
        FloatDeadzone deadzone, 
        BuoyancyParameters buoyancyParameters
    ){
        this.shipBody = shipBody;
        this.waterLine = waterLine;
        this.deadzone = deadzone;
        this.parameters = buoyancyParameters;
        this.buoyantForce = new BuoyantForce();
    }

    public BuoyantForce update(Transform[] floatPoints){
        float equil = this.waterLine.position.y;

        List<int> abovewaterPoints = this.aboveWaterIndexes(floatPoints, equil);
        List<int> underwaterPoints = this.underWaterIndexes(floatPoints, equil);

        if(abovewaterPoints.Count < 1){
            this.shipBody.useGravity = false;
        } else {
            this.shipBody.useGravity = true;
        }

        if(underwaterPoints.Count == 0){
            this.buoyantForce.Null();

            return this.buoyantForce;
        }

        this.buoyantForce.setPosition(0, 0, 0);
        foreach(int pointIndex in underwaterPoints){
            this.buoyantForce.position += floatPoints[pointIndex].position / underwaterPoints.Count;
        }

        float depth = equil - this.buoyantForce.position.y;
        this.buoyantForce.setForce(this.parameters.calculate(depth)); 

        return this.buoyantForce;
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
