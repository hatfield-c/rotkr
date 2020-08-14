using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, ILoot
{
    [SerializeField] BuoyancyManager BuoyancyManager;
    public int Value = 0;

    public void Init(int value, GameObject waterplane){
        this.Value = value;
        this.BuoyancyManager.Init(waterplane);
    }

    public GameObject GetGameObject(){
        return this.gameObject;
    }
}
