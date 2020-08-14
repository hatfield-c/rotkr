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

    protected GameObject Waterplane;

    public void Init(GameObject waterplane){
        this.Waterplane = waterplane;
    }

    public List<ILoot> CreateLoot(){
        List<ILoot> scrapList = new List<ILoot>();

        int instanceCount = Random.Range(this.MinInstances, this.MaxInstances + 1);
        int totalValue = Random.Range(this.MinTotalValue, this.MaxTotalValue + 1);
        int instanceValue = this.CalculateInstanceValue(totalValue, instanceCount);

        for(int i = 0; i < instanceCount; i++){
            GameObject scrapObject = GameObject.Instantiate(this.Prefab);

            Scrap scrap = scrapObject.GetComponent<Scrap>();
            scrap.Init(instanceValue, this.Waterplane);

            scrapList.Add(scrap);
        }

        return scrapList;
    }

    protected int CalculateInstanceValue(int totalValue, int instanceCount){
        int value = (int)( (float)totalValue / (float)instanceCount );

        if(value < 1){
            return 1;
        }

        return value;
    }
}
