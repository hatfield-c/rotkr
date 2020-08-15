using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScrapFactory : ILootFactory
{
    [Header("Prefab")]
    [SerializeField] GameObject Prefab = null;

    [Header("Instance Count")]
    [SerializeField] int MaxInstances = 1;
    [SerializeField] int MinInstances = 1;

    [Header("Possible Values")]
    [SerializeField] int MaxTotalValue = 10;
    [SerializeField] int MinTotalValue = 10;

    public List<ILoot> CreateLoot(){
        List<ILoot> scrapList = new List<ILoot>();

        int instanceCount = Random.Range(this.MinInstances, this.MaxInstances + 1);
        int totalValue = Random.Range(this.MinTotalValue, this.MaxTotalValue + 1);

        for(int i = 0; i < instanceCount; i++){
            GameObject scrapObject = GameObject.Instantiate(this.Prefab);
            Scrap scrap = scrapObject.GetComponent<Scrap>();
            scrap.Init((int)(totalValue / instanceCount));
        }

        return scrapList;
    }
}
