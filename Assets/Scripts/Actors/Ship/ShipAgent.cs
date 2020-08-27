using System.Collections;
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
    [SerializeField] float OperationalDistance = 0;
    [SerializeField] float MaxSpeed = 0;
    [SerializeField] protected float MaxAngularSpeed = 0; 

    protected Brain brain;
    protected GameObject playerObject;
    protected Rigidbody playerBody;
    protected WaterCalculator waterCalculator;

    protected Vector3 vectorBuffer = new Vector3();
    protected Vector2 vector2BufferA = new Vector2();
    protected Vector2 vector2BufferB = new Vector2();

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

        int action0 = Mathf.FloorToInt(vectorAction[0]);
        int action1 = Mathf.FloorToInt(vectorAction[1]);
        int action2 = Mathf.FloorToInt(vectorAction[2]);

        if (action0 == 1) { accel = 1f; }
        if (action0 == 2) { accel = 0.5f; }
        if (action0 == 3) { accel = -1f; }
        if (action0 == 4) { accel = -0.5f; }

        if (action1 == 1) { turn = 1f; }
        if (action1 == 2) { turn = 0.5f; }
        if (action1 == 3) { turn = -1f; }
        if (action1 == 4) { turn = -0.5f; }

        if (action2 == 1) { shoot = 1f; }

        ShipAgentActions actions = new ShipAgentActions(
            accel,
            turn,
            shoot
        );

        this.shipManager.TakeAction(actions);
    }

    public override void OnEpisodeBegin(){
        
    }

    //***
    //***   observations:
    //***       agent.forward *DOT* direction to player
    //***       agent.right *DOT* direction to player
    //***       agent.left *DOT* direction to player
    //***       player.forward *DOT* direction to agent
    //***       player.forward *DOT* agent.forward
    //***       distance from agent to player (normalized)
    //***       speed of the agent (normalized)
    //***       speed of the player (normalized)
    //***       8 sample points of the water level surrounding the ship
    //***       
    public override void CollectObservations(VectorSensor sensor){
        this.vectorBuffer.x = this.playerObject.transform.position.x - this.transform.position.x;
        this.vectorBuffer.y = this.playerObject.transform.position.y - this.transform.position.y;
        this.vectorBuffer.z = this.playerObject.transform.position.z - this.transform.position.z;

        float dotResult = Vector3.Dot(this.transform.forward, this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        dotResult = Vector3.Dot(this.transform.right, this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        dotResult = Vector3.Dot(-this.transform.right, this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        dotResult = Vector3.Dot(this.playerObject.transform.forward , -this.vectorBuffer.normalized);
        sensor.AddObservation(dotResult);

        dotResult = Vector3.Dot(this.transform.forward, this.playerObject.transform.forward);
        sensor.AddObservation(dotResult);

        sensor.AddObservation(this.vectorBuffer.magnitude / this.OperationalDistance);

        sensor.AddObservation(this.shipBody.velocity.magnitude / this.MaxSpeed);

        sensor.AddObservation(this.playerBody.velocity.magnitude / this.MaxSpeed);

        sensor.AddObservation(this.shipBody.angularVelocity.y / this.MaxAngularSpeed);

        this.vectorBuffer = this.waterSampler.GetSamplePoint(0);
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
        sensor.AddObservation(0);//this.waterCalculator.calculateHeight(this.vectorBuffer.x, this.vectorBuffer.z));

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
