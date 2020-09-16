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

    [Header("References")]
    [SerializeField] protected Rigidbody shipBody = null;
    [SerializeField] protected List<ASensor> sensors = null;

    [Header("Normalization Parameters")]
    [SerializeField] protected float operationalDistance = 0;
    [SerializeField] protected int distanceSteps = 1;
    [SerializeField] protected int dirSteps = 1;
    [SerializeField] float maxSpeed = 0;
    [SerializeField] int speedSteps = 1;

    [Header("Training")]
    [SerializeField] bool isTraining = false;

    protected Brain brain;
    protected GameObject playerObject;
    protected Rigidbody playerBody;
    protected WaterCalculator waterCalculator;

    protected float dizzyness;
    protected Vector3 vectorBuffer = new Vector3();

    public virtual void Init(
        Brain brain, 
        GameObject playerObject
        ){
        if(!this.enabled){
            return;
        }

        this.brain = brain;
        this.playerObject = playerObject;
        this.playerBody = playerObject.GetComponent<Rigidbody>();

        this.OrderSensors();

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
    //***           0 : Brake
    //***           1 : Forward 
    //***       1 : Turn Direction
    //***           0 : Do nothing
    //***           1 : Turn right
    //***           2 : Turn left 

    public override void OnActionReceived(float[] vectorAction){
        float accel = 0f;
        float turn = 0f;
        float shoot = 0f;
        float brake = 0f;

        int action0 = Mathf.FloorToInt(vectorAction[0]);
        int action1 = Mathf.FloorToInt(vectorAction[1]);

        if (action0 == 0) { brake = 1f; }
        if (action0 == 1) { accel = 1f; }

        if (action1 == 1) { turn = 1f; }
        if (action1 == 2) { turn = -1f; }

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
    //***       1 : 
    //***       
    //***      11
    public override void CollectObservations(VectorSensor sensor){
        if(this.playerObject == null) {
            return;
        }

        this.vectorBuffer = this.transform.InverseTransformPoint(this.playerObject.transform.position);

        // angle between ship forward and target
        float angle = this.PerceiveAngle(
            Vector3.forward,
            this.vectorBuffer.normalized,
            Vector3.up
        );
        sensor.AddObservation(angle);

        // angle between ship's forward and ship's velocity
        angle = this.PerceiveAngle(
            Vector3.forward,
            this.transform.InverseTransformVector(this.shipBody.velocity).normalized, 
            Vector3.up
        );
        sensor.AddObservation(angle);
        
        /*/ angle between target's velocity and this ship
        angle = this.PerceiveAngle(
            this.playerObject.transform.InverseTransformVector(this.playerBody.velocity).normalized, 
            this.playerObject.transform.InverseTransformPoint(this.transform.position).normalized, 
            Vector3.up
        );
        sensor.AddObservation(angle);*/

        // Speed of ship 
        float speed = this.DiscretizeOrigin(
            this.shipBody.velocity.magnitude,
            this.maxSpeed,
            this.speedSteps
        );
        sensor.AddObservation(speed);

        // distance to player 
        float distanceZone = this.DiscreteDistance(
            Vector3.Distance(this.transform.position, this.playerObject.transform.position)
        ); 
        sensor.AddObservation(distanceZone);

        // 1-normalization term (what I call "zero-grounding" term)
        //sensor.AddObservation(1f);

        // 6
        foreach(ASensor raySensor in this.sensors) {
            float zone = raySensor.ReadSensorPool();
            sensor.AddObservation(zone);
        }

    }

    public void ResetAgent() {
        if (!this.isTraining) {
            return; 
        }

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
    
    protected float PerceiveAngle(Vector3 start, Vector3 end, Vector3 axis) {
        float angle = Vector3.SignedAngle(start, end, axis);
        float angleSign = Mathf.Sign(angle);

        angle = (Mathf.Abs(angle) / 180f) * this.dirSteps;
        angle = angleSign > 0 ? Mathf.Ceil(angle) : Mathf.Floor(angle);
        angle = angle / this.dirSteps;
        angle = angle * angleSign;

        return angle;
    }

    protected float DiscretizeAngle(float angle) {
        float angleSign = Mathf.Sign(angle);
        float discreteAngle = (Mathf.Abs(angle) / 180f) * this.dirSteps;
        
        discreteAngle = angleSign > 0 ? Mathf.Ceil(discreteAngle) : Mathf.Floor(discreteAngle);
        discreteAngle = discreteAngle / this.dirSteps;
        discreteAngle = discreteAngle * angleSign;

        return discreteAngle;
    }

    protected float DiscretizeOrigin(float value, float maxValue, int steps) {
        float result = value / (maxValue / steps);
        result = Mathf.Ceil(result) / steps;

        return result;
    }

    protected float DiscreteDistance(float distance) {
        float distanceZone = distance / (this.operationalDistance / this.distanceSteps);
        distanceZone = Mathf.Ceil(distanceZone) / this.distanceSteps;

        return distanceZone;
    }

    protected void OrderSensors() {
        if(this.sensors.Count < 2) {
            return;
        }

        ASensor buffer;
        bool isSorted = false;

        while (!isSorted) {
            isSorted = true;

            for(int i = 0; i < this.sensors.Count - 1; i++) {
                if(this.sensors[i].GetId() > this.sensors[i + 1].GetId()) {
                    buffer = this.sensors[i];
                    this.sensors[i] = this.sensors[i + 1];
                    this.sensors[i + 1] = buffer;

                    isSorted = false;
                }
            }
        }
    }

    public override void Heuristic(float[] actionsOut) {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;

        if (Input.GetKey(KeyCode.W)) {
            actionsOut[0] = 1f;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            actionsOut[0] = 0f;
        }

        if (Input.GetKey(KeyCode.A)) {
            actionsOut[1] = 2f;
        } else if (Input.GetKey(KeyCode.D)) {
            actionsOut[1] = 1f;
        }
    }

    public static void EmptyReset() { }
}
