using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorShip : MonoBehaviour
{
    public ActorShipManager ShipManager;
    public string identity;
    [MinAttribute(1)]
    public int ComputationCost = 1;
    [MinAttribute(1)]
    public int ChallengeRating = 1;

    public Action<ActorShip> RemoveShipAction;

    void OnEnable(){
        this.ShipManager.RemoveShipAction += this.Remove;
    }

    void OnDisable(){
        this.ShipManager.RemoveShipAction -= this.Remove;
    }

    public void Remove(){
        this.RemoveShipAction?.Invoke(this);
    }

    public void Reset(){
        this.ShipManager.ResetShip();
    }
}
