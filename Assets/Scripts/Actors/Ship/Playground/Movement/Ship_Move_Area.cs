using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class Ship_Move_Area : MonoBehaviour {
    public Ship_Move_Agent shipAgent;
    public TextMeshPro rewardText;

    public Ship_Move_Target target;

    protected float dir = 1f;

    void Start(){

    }

    void Update() {
        this.rewardText.text = this.shipAgent.GetCumulativeReward().ToString();
    }

    public void ResetArea(){
        this.target.resetSelf();
    }
}
