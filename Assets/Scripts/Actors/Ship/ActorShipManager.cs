using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActorShipManager : MonoBehaviour {
    [SerializeField] LootManager lootManager = null;
    [SerializeField] HunkManager hunkManager = null;
    [SerializeField] ActorHealthManager healthManager = null;
    [SerializeField] ActorEquipment equipmentManager = null;
    [SerializeField] List<Brain> BrainList = new List<Brain>();
    [SerializeField] ShipAgent shipAgent = null;
    [SerializeField] ActorShipMovement shipMovement = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;
    [SerializeField] ActorShip actorShipReference = null;

    protected Warehouse.StorageFunction StorageFunction;

    public void Init(
        Warehouse lootWarehouse, 
        GameObject playerObject,
        GameObject waterPlane, 
        EnemyFactory.GameDifficulty Difficulty,
        Warehouse.StorageFunction storageFunction
    ){
        ShipData shipData = new ShipData();
        Brain brain = this.GetBrain(Difficulty);
        this.StorageFunction = storageFunction;

        lootManager.Init(lootWarehouse);
        hunkManager.Init(shipData.HunkDatum);
        healthManager.Init(hunkManager.GetHunkCount());

        equipmentManager.Init();
        shipAgent.Init(brain, playerObject, waterPlane.GetComponent<WaterCalculator>());
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
            this.StorageFunction(this.actorShipReference);
        });
        deathSequence.Play();
        this.shipAgent.resetFunction();
    }

    public List<IStorable> GetPossibleLoot(){
        return this.lootManager.GetPossibleLoot();
    }

    public void EnableShip(){
        this.gameObject.SetActive(true);
        this.EnableSubsystems();
    }

    public void EnableSubsystems(){
        shipMovement.SetCanMove(true);
        lootManager.EnableLoot();
        hunkManager.EnableHunks();
        healthManager.Enable();
        buoyancyManager.Enable();
    }

    public void DisableShip(){
        this.DisableSubsystems();
        this.gameObject.SetActive(false);
    }

    public void DisableSubsystems(){
        lootManager.DisableLoot();
        hunkManager.DisableHunks();
        healthManager.Disable();
        buoyancyManager.Disable();
    }   

    public Brain GetBrain(EnemyFactory.GameDifficulty difficulty){
        foreach(Brain brain in this.BrainList){
            if(brain.GetDifficulty() == difficulty){
                return brain;
            }
        }

        return this.BrainList[0];
    }

    void OnEnable(){
        this.hunkManager.HunkBroken += this.healthManager.OnHunkBreak;
        this.healthManager.DeathAction += this.KillThisShip;
    }

    void OnDisable(){
        this.hunkManager.HunkBroken -= this.healthManager.OnHunkBreak;
        this.healthManager.DeathAction -= this.KillThisShip;
    }

}
