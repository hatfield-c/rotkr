using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgent : Agent
{
    [SerializeField] ActorShipManager shipManager = null;

    protected Brain brain;

    public void Init(Brain brain){
        this.brain = brain;

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

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(0);
    }

    public override void Heuristic(float[] actionsOut){

    }
}
