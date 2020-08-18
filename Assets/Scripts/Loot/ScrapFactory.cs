using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScrapFactory : ILootFactory
{
    [Header("Prefab")]
    [SerializeField] Scrap Prefab = null;

    [Header("Instance Count")]
    [SerializeField] int MaxInstances = 1;
    [SerializeField] int MinInstances = 1;

    [Header("Possible Values")]
    [SerializeField] int MaxTotalValue = 10;
    [SerializeField] int MinTotalValue = 10;

    protected Warehouse LootWarehouse;
    protected List<ILoot> ScrapList = new List<ILoot>();

    public void Init(Warehouse lootWarehouse){
        this.LootWarehouse = lootWarehouse;
    }

    public List<ILoot> GetLoot(){
        this.ScrapList.Clear();

        int maxAvailable = this.MaxInstances;
        int availableCount = this.LootWarehouse.GetItemCount(this.Prefab.GetArchetype());

        if(availableCount < 1){
            return this.ScrapList;
        }

        if(availableCount < this.MaxInstances){
            maxAvailable = availableCount;
        }

        int instanceCount = 0;//Random.Range(this.MinInstances, maxAvailable + 1);
        int totalValue = Random.Range(this.MinTotalValue, this.MaxTotalValue + 1);
        int instanceValue = this.CalculateInstanceValue(totalValue, instanceCount);

        for(int i = 0; i < instanceCount; i++){
            Scrap scrap = (Scrap)this.LootWarehouse.FetchItem(this.Prefab.GetArchetype());
            scrap.Enable();
            scrap.SetValue(instanceValue);

            this.ScrapList.Add(scrap);
        }

        return this.ScrapList;
    }

    public Scrap GetPrefab(){
        return this.Prefab;
    }

    protected int CalculateInstanceValue(int totalValue, int instanceCount){
        int value = (int)( (float)totalValue / (float)instanceCount );

        if(value < 1){
            return 1;
        }

        return value;
    }
}
