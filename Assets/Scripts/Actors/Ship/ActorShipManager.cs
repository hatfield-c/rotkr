using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorShipManager : MonoBehaviour {

    [SerializeField] HunkManager hunkManager = null;
    [SerializeField] ActorEquipment equipmentManager = null;
    [SerializeField] ShipAgent shipAgent = null;
    [SerializeField] ActorShipMovement shipMovement = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;

    [SerializeField] List<Brain> BrainList = new List<Brain>();

    public void Init(GameObject waterPlane, Director.Difficulty Difficulty)
    {
        ShipData shipData = new ShipData();

        Brain brain = this.BrainList[0];

        hunkManager.Init(shipData.HunkDatum);
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

}
