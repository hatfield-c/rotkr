using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour {
    public ActorShip Spawn(ActorShip shipBlueprint, SpawnParameters parameters, Vector3 position){

        GameObject shipObject = Instantiate(
            shipBlueprint.gameObject, 
            position, 
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
