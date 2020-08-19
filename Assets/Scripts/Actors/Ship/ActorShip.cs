using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorShip : MonoBehaviour, IStorable
{
    public ActorShipManager ShipManager;
    public string identity;
    [MinAttribute(1)]
    public int ComputationCost = 1;
    [MinAttribute(1)]
    public int ChallengeRating = 1;

    public Action<ActorShip> RemoveShipAction;

    public GameObject GetMyGameObject(){
        return this.gameObject;
    }

    public string GetArchetype(){
        return this.identity;
    }

    public void Enable(){
        this.ShipManager.EnableShip();
    }

    public void Disable(){
        this.ShipManager.DisableShip();
    }

    void OnEnable(){
        this.ShipManager.RemoveShipAction += this.Remove;
    }

    void OnDisable(){
        this.ShipManager.RemoveShipAction -= this.Remove;
    }

    public void Remove(){
        this.RemoveShipAction?.Invoke(this);
    }

}
