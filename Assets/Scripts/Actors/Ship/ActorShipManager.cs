using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActorShipManager : MonoBehaviour {

    public Action RemoveShipAction;

    [SerializeField] LootManager lootManager = null;
    [SerializeField] HunkManager hunkManager = null;
    [SerializeField] ActorHealthManager healthManager = null;
    [SerializeField] ActorEquipment equipmentManager = null;
    [SerializeField] List<Brain> BrainList = new List<Brain>();
    [SerializeField] ShipAgent shipAgent = null;
    [SerializeField] ActorShipMovement shipMovement = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;

    public void Init(GameObject waterPlane, EnemyFactory.GameDifficulty Difficulty)
    {
        ShipData shipData = new ShipData();

        Brain brain = this.BrainList[0];

        lootManager.Init(waterPlane);
        hunkManager.Init(shipData.HunkDatum);
        healthManager.Init(hunkManager.GetHunkCount());

        equipmentManager.Init();
        shipAgent.Init(brain);
        shipMovement.Init(waterPlane);
        buoyancyManager.Init(waterPlane);
    }

    public void TakeAction(ShipAgentActions actions){
        this.shipMovement.ApplyControls(
            actions.GetAcceleration(),
            actions.GetTurnDirection()
        );

        this.equipmentManager.Activate(actions.GetShoot());
    }

    public void KillThisShip(){
        if(buoyancyManager.IsSinking())
            return;

        buoyancyManager.ActivateSinking();
        shipMovement.SetCanMove(false);
        lootManager.DropLoot();

        Sequence deathSequence = DOTween.Sequence();
        deathSequence.InsertCallback(healthManager.DeathDelay, () => {
            this.RemoveShipAction?.Invoke();
        });
        deathSequence.Play();
    }

    public void ResetShip(){
        lootManager.ResetLoot();
        hunkManager.ResetHunks();
        healthManager.ResetHealth();
        buoyancyManager.Reset();
    }

    public void DisableShip(){
        this.gameObject.SetActive(false);
    }

    public void EnableShip(){
        this.gameObject.SetActive(true);
    }

    void OnEnable(){
        this.hunkManager.HunkBroken += this.healthManager.OnHunkBreak;
        this.healthManager.DeathAction += this.KillThisShip;
        this.RemoveShipAction += this.DisableShip;
    }

    void OnDisable(){
        this.hunkManager.HunkBroken -= this.healthManager.OnHunkBreak;
        this.healthManager.DeathAction -= this.KillThisShip;
        this.RemoveShipAction -= this.DisableShip;
    }

}
