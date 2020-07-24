using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Ship_Move_Agent : Agent
{
    public Ship_Move_Area shipArea;

    public ActorShipMovement moveControl;
    public ActorShipManager shipManager;

    protected ActorEquipment equipment;

    protected InputMaster controls;
    protected InputRunner controlRun;

    protected Rigidbody rb;
    protected Vector3 startPosition;
    protected Vector3 startRotation;
    protected Transform transform;

    protected float accel;
    protected float turnRate;
    protected float shoot; 

    public override void Initialize(){
        base.Initialize();

        this.rb = this.GetComponent<Rigidbody>();
        this.transform = this.GetComponent<Transform>();
        this.startPosition = this.transform.position;
        this.startRotation = this.transform.eulerAngles;

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
            this.AddReward(1f / 10000f);

            //this.EndEpisode();
        }

    }

    /***
    ***     float vectorAction[]
    ***         0: accel
    ***         1 : turn rate
    ***/
    public override void OnActionReceived(float[] vectorAction){
        float accel = vectorAction[0];
        float turn = vectorAction[1];

        this.moveControl.ControlShip(accel, turn);

        //this.AddReward(-1f / 5000);
        this.AddReward(
            -Vector3.Distance(
                this.transform.position,
                this.shipArea.target.getTransform().position
            ) / (1000 * 10000)
        );
    }

    public override void OnEpisodeBegin(){
        this.shipArea.ResetArea();
        this.rb.velocity = Vector3.zero;
        this.transform.position = this.startPosition;
        this.transform.eulerAngles = this.startRotation;
        //this.lastShot = 0f;
    }

    public override void CollectObservations(VectorSensor sensor){
        Vector3 targetPos = (this.shipArea.target.getTransform().position - this.shipArea.transform.position) / 1000f;
        Vector3 targetVel = this.shipArea.getRb().velocity / this.shipArea.maxSpeed;

        Vector3 selfPos = (this.transform.position - this.shipArea.transform.position) / 1000f;
        Vector3 selfVel = this.rb.velocity / 500f;
        Vector3 selfRot = this.transform.rotation.eulerAngles / 360f;

        float distance = Vector3.Distance(
            this.transform.position,
            this.shipArea.target.getTransform().position
        ) / 1000f;

        sensor.AddObservation(targetPos.x);
        sensor.AddObservation(targetPos.y);
        sensor.AddObservation(targetPos.z);
        sensor.AddObservation(targetVel.x);
        sensor.AddObservation(targetVel.y);
        sensor.AddObservation(targetVel.z);

        sensor.AddObservation(selfPos.x);
        sensor.AddObservation(selfPos.y);
        sensor.AddObservation(selfPos.z);
        sensor.AddObservation(selfVel.x);
        sensor.AddObservation(selfVel.y);
        sensor.AddObservation(selfVel.z);

        sensor.AddObservation(selfRot.x);
        sensor.AddObservation(selfRot.y);
        sensor.AddObservation(selfRot.z);
        sensor.AddObservation(distance);

        Vector3 difference = this.shipArea.target.getTransform().position - this.transform.position;
        float angle = Vector3.Angle(this.transform.forward, difference);

        sensor.AddObservation(angle);

        // Observations
        // target pos x
        // target pos y
        // target pos z
        // target vel x
        // target vel y
        // target vel z
        // self pos x
        // self pos y
        // self pos z
        // self vel x
        // self vel y
        // self vel z
        // self rotation x
        // self rotation y
        // self rotation z
        // distance between two entities
        // angle between front of ship and the target
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = this.accel;
        actionsOut[1] = this.turnRate;
    }

    public void HeuristicMovement(Vector2 inputDirection){
        //this.moveControl.ControlShip(inputDirection.y, inputDirection.x);
        this.accel = inputDirection.y;
        this.turnRate = inputDirection.x;
    }

    public void HeuristicShoot(){
        this.shoot = 1f;
    }

}
