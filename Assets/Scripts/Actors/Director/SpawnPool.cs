using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour {
    public ActorShip Spawn(ActorShip shipBlueprint, SpawnParameters parameters){

        GameObject shipObject = Instantiate(
            shipBlueprint.gameObject, 
            new Vector3(0, 0, 0), 
            Quaternion.identity, 
            null
        );

        ActorShip instance = shipObject.GetComponent<ActorShip>();
        instance.ShipManager.Init(
            parameters.WaterPlane,
            parameters.Difficulty
        );

        return instance;
    }
}
