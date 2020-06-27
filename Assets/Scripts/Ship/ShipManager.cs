using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    public HunkManager hunkManager;

    void Start()
    {
        this.hunkManager.Init(null);
    }

    void Update(){
        
    }
}
