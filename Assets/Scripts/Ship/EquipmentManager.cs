using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentManager
{
    public GameObject[] equipmentList;
    public Transform ammunitionStorage;

    public void Init(InputMaster controls){
        foreach(GameObject equipmentObject in this.equipmentList){
            IEquipment equipment = equipmentObject.GetComponent<IEquipment>();
            equipment.registerInput(controls);
        }

        this.ammunitionStorage.parent = null;
    }
    public void UpdateCannonReloads(float newReloadRatio){
        foreach (GameObject equipmentObject in this.equipmentList){
            Cannon cannon = equipmentObject.GetComponent<Cannon>();
            if (cannon == null)
                continue;
            cannon.UpdateReloadSpeed(newReloadRatio);
        }
    }
}
