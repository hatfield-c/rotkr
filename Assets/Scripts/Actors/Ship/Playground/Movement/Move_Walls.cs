using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Move_Walls : MonoBehaviour {

    public Ship_Move_Agent agent;

    void OnCollisionStay(Collision other){
        if(other.collider.tag == "waso_move_Agent"){
            this.agent.AddReward(-0.5f);
        }
    }

}