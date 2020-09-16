using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorEquipment
{
    public ActorCannon[] gunList;
    public Transform ammunitionStorage;
    public float activationThreshold = 0.5f;

    public void Init(GameObject playerObject){
        this.ammunitionStorage.parent = null;

        foreach(ActorCannon gun in this.gunList) {
            gun.Init(playerObject);
        }
    }

    public void Activate(float shoot){
        if(shoot < this.activationThreshold){
            return;
        }

        if (!this.CanShoot()) {
            return;
        }

        foreach(ActorCannon gun in this.gunList){
            gun.lightFuse();
        }
    }

    public bool CanShoot() {
        foreach(ActorCannon gun in this.gunList) {
            if (!gun.IsLoaded()) {
                return false;
            }
        }

        return true;
    }

    public void Enable() {
        foreach(ActorCannon gun in this.gunList) {
            gun.Enable();
        }
    }

    public void Disable() {
        foreach (ActorCannon gun in this.gunList) {
            gun.Disable();
        }
    }

    public void DestroyEquipment(){
        if(this.ammunitionStorage == null)
            return;

        GameObject.Destroy(this.ammunitionStorage.gameObject);
    }
}
