using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, ILoot, IStorable
{
    [SerializeField] BuoyancyManager BuoyancyManager = null;
    [SerializeField] Rigidbody Rigidbody = null;
    [SerializeField] string identity = "scrap";
    public int Value = 0;

    public void Init(GameObject waterplane){
        this.BuoyancyManager.Init(waterplane);
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

    public void DestroySelf(){
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        ShipManager ship = other.GetComponentInParent<ShipManager>();
        if (ship == null) return;
        ship.ScrapPickUp(Value);
        Value = 0;
        Destroy(gameObject, .1f);
    }
}
