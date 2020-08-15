using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, ILoot
{
    public int Value = 0;

    public void Init(int value){
        this.Value = value;
    }

    public GameObject GetGameObject(){
        return this.gameObject;
    }
}
