using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, ILoot, IStorable
{
    [SerializeField] BuoyancyManager BuoyancyManager = null;
    [SerializeField] Rigidbody Rigidbody = null;
    [SerializeField] string identity = "scrap";
    public int Value = 0;

    public Warehouse.StorageFunction StorageFunction;

    public void Init(GameObject waterplane, Warehouse.StorageFunction storageFunction){
        this.BuoyancyManager.Init(waterplane);
        this.StorageFunction = storageFunction;
    }

    public GameObject GetGameObject(){
        return this.gameObject;
    }

    public Rigidbody GetRigidbody(){
        return this.Rigidbody;
    }

    public void SetValue(int value){
        this.Value = value;
    }

    public int GetValue(){
        return this.Value;
    }

    public GameObject GetMyGameObject() {
        return this.gameObject;
    }

    public string GetArchetype() {
        return this.identity;
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    public void ReturnToPool(){
        this.StorageFunction((IStorable)this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        ShipManager ship = other.GetComponentInParent<ShipManager>();
        if (ship == null) return;
        ship.AddScrap(Value);
        Value = 0;
        this.ReturnToPool();
    }
}
