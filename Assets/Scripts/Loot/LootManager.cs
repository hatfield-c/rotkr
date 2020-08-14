using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootManager
{
    [SerializeField] Transform LootSpawn = null;
    [SerializeField] ScrapFactory ScrapFactory = null;

    protected List<ILoot> LootList = new List<ILoot>();

    public void Init(GameObject waterplane){
        this.ScrapFactory.Init(waterplane);

        this.GenerateLoot(this.ScrapFactory);

        foreach(ILoot iloot in this.LootList){
            GameObject lootObject = iloot.GetGameObject();
            lootObject.transform.parent = this.LootSpawn;
            lootObject.SetActive(false);
        }
    }

    protected void GenerateLoot(ILootFactory lootFactory){
        List<ILoot> loot = lootFactory.CreateLoot();

        if(loot.Count < 0){
            return;
        }

        this.LootList.AddRange(loot);
    }
}
