using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, ILoot
{
    [SerializeField] BuoyancyManager BuoyancyManager = null;
    [SerializeField] Rigidbody Rigidbody = null;
    public int Value = 0;

    public void Init(int value, GameObject waterplane){
        this.Value = value;
        this.BuoyancyManager.Init(waterplane);
    }

    public GameObject GetGameObject(){
        return this.gameObject;
    }

    public Rigidbody GetRigidbody(){
        return this.Rigidbody;
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
