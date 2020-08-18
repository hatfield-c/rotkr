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

    public void Init(Warehouse lootWarehouse, GameObject waterPlane, EnemyFactory.GameDifficulty Difficulty)
    {
        ShipData shipData = new ShipData();

        Brain brain = this.BrainList[0];

        lootManager.Init(lootWarehouse);
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

    public List<IStorable> GetPossibleLoot(){
        return this.lootManager.GetPossibleLoot();
    }

    public void EnableShip(){
        lootManager.EnableLoot();
        hunkManager.EnableHunks();
        healthManager.Enable();
        buoyancyManager.Enable();

        this.gameObject.SetActive(true);
    }

    public void DisableShip(){
        lootManager.DisableLoot();
        hunkManager.DisableHunks();
        healthManager.Disable();
        buoyancyManager.Disable();

        this.gameObject.SetActive(false);
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
