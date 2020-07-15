using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    public HunkManager hunkManager;
    public EquipmentManager equipmentManager;

    protected InputMaster controls;

    void Awake(){
        this.controls = new InputMaster();
    }

    void Start()
    {
        this.hunkManager.Init(null);
        this.equipmentManager.Init(this.controls);
    }

    void Update(){
        
    }

    void OnEnable()
    {
        this.controls.Enable();
    }

    void OnDisable()
    {
        this.controls.Disable();
    }
}
