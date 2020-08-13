using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Ship_Move_Agent : Agent
{
    /*public Ship_Move_Area shipArea;

    public ActorShipMovement moveControl;
    public ActorShipManager shipManager;

    protected ActorEquipment equipment;

    protected InputMaster controls;
    protected InputRunner controlRun;

    protected Rigidbody rb;
    protected Vector3 startPosition;
    protected Vector3 startRotation;
    protected Transform trans;

    protected float accel;
    protected float turnRate;
    protected float shoot; 

    public override void Initialize(){
        base.Initialize();

        this.rb = this.GetComponent<Rigidbody>();
        this.trans = this.GetComponent<Transform>();
        this.startPosition = this.trans.position;
        this.startRotation = this.trans.eulerAngles;

        this.equipment = this.shipManager.equipmentManager;

        this.controlRun = null;
        this.controls = new InputMaster();

        //this.controlRun = new InputRunner();
        //this.controls = this.controlRun.controls;

        this.controls.Player.Movement.performed += context => this.HeuristicMovement(context.ReadValue<Vector2>());
        this.controls.Player.Shoot.performed += context => this.HeuristicShoot();
    }

    void FixedUpdate(){

        if(this.shipArea.target.isHit){
            Debug.Log("hit!");
            this.AddReward(1f / 5000f);
        }

    }*/

    /***
    ***     float vectorAction[]
    ***         0: accel
    ***         1 : turn rate
    ***/ 
    /*
    public override void OnActionReceived(float[] vectorAction){
        float accel = vectorAction[0];
        float turn = vectorAction[1];

        this.moveControl.ControlShip(accel, turn);

        float dist = Vector3.Distance(
            this.trans.position,
            this.shipArea.target.getTransform().position
        );

        this.AddReward(
            -( dist / (1000f * 10000f))
        );
    }

    public override void OnEpisodeBegin(){
        this.shipArea.ResetArea();
        this.rb.velocity = Vector3.zero;
        this.trans.position = this.startPosition;
        this.trans.eulerAngles = this.startRotation;
    }

    public override void CollectObservations(VectorSensor sensor){
        Vector3 selfVel = this.rb.velocity;
        Vector3 difference = this.shipArea.target.getTransform().position - this.trans.position;
        float angle = Vector3.Angle(this.trans.forward, difference);

        float distance = Vector3.Distance(
            this.trans.position,
            this.shipArea.target.getTransform().position
        ) / 1000f;

        sensor.AddObservation(this.trans.forward.x);
        sensor.AddObservation(this.trans.forward.y);
        sensor.AddObservation(this.trans.forward.z);

        sensor.AddObservation(selfVel.magnitude / 500f);
        sensor.AddObservation(selfVel.normalized.x);
        sensor.AddObservation(selfVel.normalized.y);
        sensor.AddObservation(selfVel.normalized.z);

        sensor.AddObservation(distance);

        sensor.AddObservation(difference.x / 1000);
        sensor.AddObservation(difference.y / 1000);
        sensor.AddObservation(difference.z / 1000);
        sensor.AddObservation(angle);
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = this.accel;
        actionsOut[1] = this.turnRate;
    }

    public void HeuristicMovement(Vector2 inputDirection){
        this.accel = inputDirection.y;
        this.turnRate = inputDirection.x;
    }

    public void HeuristicShoot(){
        this.shoot = 1f;
    }
    */

}
