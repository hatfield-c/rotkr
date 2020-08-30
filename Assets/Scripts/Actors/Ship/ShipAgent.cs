﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgent : Agent
{
    public delegate void ResetFunction();

    public ActorShipManager shipManager = null;
    public ResetFunction resetFunction = EmptyReset;

    [SerializeField] protected Rigidbody shipBody = null;
    [SerializeField] WaterSampler waterSampler = null;

    [Header("Normalization Parameters")]
    [SerializeField] protected float OperationalDistance = 0;
    [SerializeField] float MaxSpeed = 0;
    [SerializeField] protected float MaxAngularSpeed = 0; 

    protected Brain brain;
    protected GameObject playerObject;
    protected Rigidbody playerBody;
    protected WaterCalculator waterCalculator;

    protected Vector3 vectorBuffer = new Vector3();

    public void Init(
        Brain brain, 
        GameObject playerObject,
        WaterCalculator waterCalculator
        ){
        if(!this.enabled){
            return;
        }

        this.brain = brain;
        this.playerObject = playerObject;
        this.playerBody = playerObject.GetComponent<Rigidbody>();
        this.waterCalculator = waterCalculator;

        NNBehaviour combat = brain.CombatBehavior;

        this.SetModel(
            combat.name,
            combat.neuralNetwork,
            combat.inferenceDevice
        );
    }

    //***
    //***   vectorAction:
    //***       0 : Acceleration
    //***           0 : Do nothing
    //***           1 : Forward fast
    //***           2 : Forward slow
    //***           3 : Backward fast
    //***           4 : Backward slow
    //***       1 : Turn Direction
    //***           0 : Do nothing
    //***           1 : Turn right fast
    //***           2 : Turng right slow
    //***           3 : Turn left fast
    //***           4 : Turn left slow
    //***       2 : Shoot
    //***           0 : Do nothing
    //***           1 : Activate cannons

    public override void OnActionReceived(float[] vectorAction){
        float accel = 0f;
        float turn = 0f;
        float shoot = 0f;
        float brake = 0f;

        int action0 = Mathf.FloorToInt(vectorAction[0]);
        int action1 = Mathf.FloorToInt(vectorAction[1]);
        int action2 = Mathf.FloorToInt(vectorAction[2]);
        int action3 = Mathf.FloorToInt(vectorAction[3]);

        if (action0 == 1) { accel = 1f; }
        if (action0 == 2) { accel = -1f; }

        if (action1 == 1) { turn = 1f; }
        if (action1 == 2) { turn = -1f; }

        if (action2 == 1) { shoot = 1f; }

        if(action3 == 1) { brake = 1f; }

        ShipAgentActions actions = new ShipAgentActions(
            accel,
            turn,
            shoot,
            brake
        );

        this.shipManager.TakeAction(actions);
    }

    public override void OnEpisodeBegin(){
        
    }

    //***
    //***   observations:
    //***       1 : agent.forward *DOT* direction to player
    //***       1 : agent.right *DOT* direction to player
    //***       1 : player.velocity *DOT* direction to agent
    //***       1 : agent velocity *DOT* direction to player
    //**        3 : agent velocity (normalized)
    //***       1 : distance from agent to player (normalized)
    //***       1 : speed of the agent (normalized)
    //***       1 : speed of the player (normalized)
    //**        1 : turn speed of agent (normalized)
    //***       8 : sample points of the water level surrounding the ship
    //***      19
    public override void CollectObservations(VectorSensor sensor){
        this.vectorBuffer = this.transform.InverseTransformPoint(this.playerObject.transform.position);

        /*/ transform.forward * direction to player
        float dotResult = Vector3.Dot(this.transform.forward, this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        // transform.right * direction to player
        dotResult = Vector3.Dot(this.transform.right, this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        // player.velocity * direction to agent (from player)
        dotResult = Vector3.Dot(
            this.playerObject.transform.InverseTransformVector(this.playerBody.velocity).normalized, 
            this.playerObject.transform.InverseTransformPoint(this.transform.position).normalized
        );
        sensor.AddObservation(dotResult);

        // velocity * direction to player
        dotResult = Vector3.Dot(
            this.transform.InverseTransformVector(this.shipBody.velocity.normalized), 
            this.vectorBuffer.normalized
        );
        sensor.AddObservation(dotResult);

        // velocity components (x, y, z)
        sensor.AddObservation(
            this.transform.InverseTransformVector(this.shipBody.velocity).normalized
        );

        // distance to player
        sensor.AddObservation(this.vectorBuffer.magnitude / this.OperationalDistance);

        // speed of agent
        sensor.AddObservation(this.shipBody.velocity.magnitude / this.MaxSpeed);

        // speed of player
        sensor.AddObservation(this.playerBody.velocity.magnitude / this.MaxSpeed);

        // turn rate of agent
        sensor.AddObservation(this.shipBody.angularVelocity.y / this.MaxAngularSpeed);*/

        // 8
        // direction to player
        sensor.AddObservation(this.vectorBuffer.normalized);

        // velocity components (x, y, z)
        sensor.AddObservation(
            this.transform.InverseTransformVector(this.shipBody.velocity).normalized
        );

        // transform.forward * direction to player
        float angle = Vector3.SignedAngle(Vector3.forward.normalized, this.vectorBuffer.normalized, Vector3.up);
        sensor.AddObservation(angle / 180);

        // angle between agent y and player y
        angle = Vector3.SignedAngle(Vector3.forward, this.vectorBuffer.normalized, Vector3.forward);
        sensor.AddObservation(angle / 180);

        // velocity * direction to player
        angle = Vector3.Dot(
            this.transform.InverseTransformVector(this.shipBody.velocity.normalized),
            this.vectorBuffer.normalized
        );
        sensor.AddObservation(angle);

        // distance to player
        sensor.AddObservation(this.vectorBuffer.magnitude / this.OperationalDistance);

        /*this.vectorBuffer = this.waterSampler.GetSamplePoint(0);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(1);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(2);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(3);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(4);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(5);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(6);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));
        this.vectorBuffer = this.waterSampler.GetSamplePoint(7);
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));*/

    }

    public void ResetAgent() {
        this.shipBody.velocity = Vector3.zero;
        this.shipBody.angularVelocity = Vector3.zero;
        this.transform.eulerAngles = new Vector3(
            0,
            Random.Range(0f, 360f),
            0
        );

        this.shipManager.DisableSubsystems();
        this.shipManager.EnableSubsystems();
    }

    public override void Heuristic(float[] actionsOut){

    }

    public static void EmptyReset() {}
}
