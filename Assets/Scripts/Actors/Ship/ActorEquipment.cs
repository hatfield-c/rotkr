using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorEquipment
{
    public Cannon[] gunList;
    public Transform ammunitionStorage;

    public void Init(){
        this.ammunitionStorage.parent = null;
    }

    public void activate(){
        foreach(Cannon gun in this.gunList){
            gun.lightFuse();
        }
    }
}
