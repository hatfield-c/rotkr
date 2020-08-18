using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootManager
{
    [Header("References")]
    [SerializeField] Transform LootSpawn = null;
    [SerializeField] ScrapFactory ScrapFactory = null;

    [Header("Drop Parameters")]
    [SerializeField] float DropForce = 500f;
    [SerializeField] float DropAngle = 45f;

    public delegate void StorageFunction(IStorable iStorable);

    protected List<ILoot> LootList = new List<ILoot>();
    protected Vector3 directionBuffer = new Vector3();
    protected Vector3 angleBuffer = new Vector3();

    protected List<ILoot> listBuffer = new List<ILoot>();

    public void Init(Warehouse lootWarehouse){
        this.ScrapFactory.Init(lootWarehouse);
    }

    public void ResetLoot(){
        this.LootList.Clear();
        this.GenerateLoot(this.ScrapFactory);

        GameObject objectBuffer;
        foreach(ILoot iloot in this.LootList){
            objectBuffer = iloot.GetGameObject();
            
            objectBuffer.transform.parent = this.LootSpawn;
            objectBuffer.transform.position = this.LootSpawn.position;
            objectBuffer.SetActive(false);
        }
    }

    public void DropLoot(){
        GameObject objectBuffer;
        Rigidbody bodyBuffer;

        foreach(ILoot iloot in this.LootList){
            objectBuffer = iloot.GetGameObject();
            bodyBuffer = iloot.GetRigidbody();

            objectBuffer.SetActive(true);
            objectBuffer.transform.parent = null;

            this.angleBuffer.y = Random.Range(0f, 360f);
            objectBuffer.transform.eulerAngles = this.angleBuffer;
            
            bodyBuffer.AddForce(this.GetRandomDirection() * this.DropForce);
        }
    }

    public List<IStorable> GetPossibleLoot(){
        List<IStorable> prefabList = new List<IStorable>();

        prefabList.Add((IStorable)ScrapFactory.GetPrefab());

        return prefabList;
    }

    protected void GenerateLoot(ILootFactory lootFactory){
        this.listBuffer = lootFactory.GetLoot();

        if(this.listBuffer.Count < 0){
            return;
        }

        this.LootList.AddRange(listBuffer);
    }

    protected Vector3 GetRandomDirection(){
        this.directionBuffer.x = Random.Range(-1f, 1f);
        this.directionBuffer.y = 0;
        this.directionBuffer.z = Random.Range(-1f, 1f);

        return (
            Quaternion.AngleAxis(
                -this.DropAngle, 
                Vector3.Cross(Vector3.up, this.directionBuffer)
            ) * this.directionBuffer
        ).normalized;
    }

}
