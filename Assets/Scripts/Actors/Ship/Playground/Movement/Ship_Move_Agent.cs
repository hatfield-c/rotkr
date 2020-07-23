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

    protected float accel;
    protected float turnRate;
    protected float shoot;

    protected float lastShot = 0f;

    public override void Initialize(){
        base.Initialize();

        this.equipment = this.shipManager.equipmentManager;

        //this.controlRun = new InputRunner();
        this.controls = new InputMaster();//this.controlRun.controls;

        this.controls.Player.Movement.performed += context => this.HeuristicMovement(context.ReadValue<Vector2>());
        this.controls.Player.Shoot.performed += context => this.HeuristicShoot();
    }

    void FixedUpdate(){
        this.lastShot += Time.deltaTime;

        if(this.shipArea.target.isHit){
            Debug.Log("hit!");
            this.AddReward(5f);
            this.shipArea.ResetArea();
            this.lastShot = 0f;
            //this.EndEpisode();
        }
    }

    /***
    ***     float vectorAction[]
    ***         0: shoot/hold fire
    ***/
    public override void OnActionReceived(float[] vectorAction){
        float shootNow = vectorAction[0];

        if(shootNow > 0.9f){
            this.equipment.activate();
        }

        this.AddReward(-1f / 2000);
        //this.EndEpisode();
    }

    public override void OnEpisodeBegin(){
        this.shipArea.ResetArea();
        this.lastShot = 0f;
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(this.lastShot);

        // Observations
        // target pos x
        // target pos y
        // target pos z
        // target vel x
        // target vel y
        // target vel z
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = this.shoot;

        this.shoot = 0f;
    }

    public void HeuristicMovement(Vector2 inputDirection){
        //this.moveControl.ControlShip(inputDirection.y, inputDirection.x);
    }

    public void HeuristicShoot(){
        //this.equipment.activate();
        this.shoot = 1f;
    }

}
