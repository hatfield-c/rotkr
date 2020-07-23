using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorShipManager : MonoBehaviour {
    
    public HunkManager hunkManager;
    public ActorEquipment equipmentManager;
    public ActorShipMovement shipMovement;
    public BuoyancyManager buoyancyManager;

    protected InputMaster controls;

    void Start() {
        this.equipmentManager.Init();
        hunkManager.Init(null);
    }

}
