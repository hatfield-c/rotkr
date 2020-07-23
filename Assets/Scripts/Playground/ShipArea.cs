using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class ShipArea : MonoBehaviour {
    public ShipAgent shipAgent;
    public TextMeshPro rewardText;

    public ShipTarget target;

    void Start(){

    }

    void Update() {
        this.rewardText.text = this.shipAgent.GetCumulativeReward().ToString();
    }

    public void ResetArea(){
        this.target.resetSelf();
    }
}
