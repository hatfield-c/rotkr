using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorEquipment
{
    public Cannon[] gunList;
    public Transform ammunitionStorage;
    public float activationThreshold = 0.5f;

    public void Init(){
        this.ammunitionStorage.parent = null;
    }

    public void Activate(float shoot){
        if(shoot < this.activationThreshold){
            return;
        }

        foreach(Cannon gun in this.gunList){
            gun.lightFuse();
        }
    }

    public void Enable() {
        foreach(Cannon gun in this.gunList) {
            gun.Enable();
        }
    }

    public void Disable() {
        foreach (Cannon gun in this.gunList) {
            gun.Disable();
        }
    }

    public void DestroyEquipment(){
        if(this.ammunitionStorage == null)
            return;

        GameObject.Destroy(this.ammunitionStorage.gameObject);
    }
}
