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

    [Header("Normalization Parameters")]
    [SerializeField] float OperationalDistance = 0;
    [SerializeField] float MaxSpeed = 0;
    [SerializeField] float MaxAngularSpeed = 0; 

    protected Brain brain;
    protected GameObject playerObject;
    protected Rigidbody playerBody;

    protected Vector3 vectorBuffer;
    protected Vector2 vector2BufferA;
    protected Vector2 vector2BufferB;

    public void Init(Brain brain, GameObject playerObject){
        if(!this.enabled){
            return;
        }

        this.brain = brain;
        this.playerObject = playerObject;
        this.playerBody = playerObject.GetComponent<Rigidbody>();

        NNBehaviour patrol = brain.PatrolBehavior;

        this.SetModel(
            patrol.name,
            patrol.neuralNetwork,
            patrol.inferenceDevice
        );
    }

    //***
    //***   vectorAction:
    //***       0 : Acceleration
    //***       1 : Turn Direction
    //***       2 : Shoot
    public override void OnActionReceived(float[] vectorAction){

        ShipAgentActions actions = new ShipAgentActions(
            vectorAction[0],
            vectorAction[1],
            vectorAction[2]
        );

        this.shipManager.TakeAction(actions);
    }

    public override void OnEpisodeBegin(){
        
    }

    //***
    //***   observations:
    //***       Position of player (relative to agent)
    //***       Distance of player from agent
    //***       Direction of player movement
    //***       Speed of player
    //***       Direction player is facing
    //***       Angular velocity of player
    //***       Direction of agent movement
    //***       Speed of agent
    //***       Angular velocity of agent
    //***       Angle between front of agent and the player
    //***       Angle between right side of agent and the player
    //***       Angle between left side of agent and the player
    //***
    public override void CollectObservations(VectorSensor sensor){
        this.vectorBuffer.x = this.playerObject.transform.position.x - this.transform.position.x;
        this.vectorBuffer.y = this.playerObject.transform.position.y - this.transform.position.y;
        this.vectorBuffer.z = this.playerObject.transform.position.z - this.transform.position.z;

        sensor.AddObservation(this.vectorBuffer.x / this.OperationalDistance);
        sensor.AddObservation(this.vectorBuffer.y / this.OperationalDistance);
        sensor.AddObservation(this.vectorBuffer.z / this.OperationalDistance);

        sensor.AddObservation(this.vectorBuffer.magnitude / this.OperationalDistance);

        sensor.AddObservation(this.playerBody.velocity.x / this.MaxSpeed);
        sensor.AddObservation(this.playerBody.velocity.y / this.MaxSpeed);
        sensor.AddObservation(this.playerBody.velocity.z / this.MaxSpeed);

        sensor.AddObservation(this.playerBody.velocity.magnitude / this.MaxSpeed);

        sensor.AddObservation(this.playerBody.transform.forward.x);
        sensor.AddObservation(this.playerBody.transform.forward.y);
        sensor.AddObservation(this.playerBody.transform.forward.z);

        sensor.AddObservation(this.playerBody.angularVelocity.x / this.MaxAngularSpeed);
        sensor.AddObservation(this.playerBody.angularVelocity.y / this.MaxAngularSpeed);
        sensor.AddObservation(this.playerBody.angularVelocity.z / this.MaxAngularSpeed);

        sensor.AddObservation(this.shipBody.velocity.x / this.MaxSpeed);
        sensor.AddObservation(this.shipBody.velocity.y / this.MaxSpeed);
        sensor.AddObservation(this.shipBody.velocity.z / this.MaxSpeed);

        sensor.AddObservation(this.shipBody.velocity.magnitude / this.MaxSpeed);

        sensor.AddObservation(this.shipBody.angularVelocity.x / this.MaxAngularSpeed);
        sensor.AddObservation(this.shipBody.angularVelocity.y / this.MaxAngularSpeed);
        sensor.AddObservation(this.shipBody.angularVelocity.z / this.MaxAngularSpeed);

        float angle = Vector3.SignedAngle(
            this.transform.forward,
            this.vectorBuffer,
            Vector3.up
        );
        sensor.AddObservation(angle / 180);

        angle = Vector3.SignedAngle(
            this.transform.right,
            this.vectorBuffer,
            this.transform.forward
        );
        sensor.AddObservation(angle / 180); 

        angle = Vector3.SignedAngle(
            -this.transform.right,
            this.vectorBuffer,
            this.transform.forward
        );
        sensor.AddObservation(angle / 180);
    }

    public override void Heuristic(float[] actionsOut){

    }

    public static void EmptyReset() {}
}
