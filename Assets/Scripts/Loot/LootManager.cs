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

    protected List<ILoot> LootList = new List<ILoot>();
    protected Vector3 directionBuffer = new Vector3();
    protected Vector3 angleBuffer = new Vector3();

    public void Init(GameObject waterplane){
        this.ScrapFactory.Init(waterplane);

        this.ResetLoot();
    }

    public void ResetLoot(){
        this.DestroyLoot();
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

    protected void GenerateLoot(ILootFactory lootFactory){
        List<ILoot> loot = lootFactory.CreateLoot();

        if(loot.Count < 0){
            return;
        }

        this.LootList.AddRange(loot);
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

    protected void DestroyLoot(){
        foreach(ILoot iloot in this.LootList){
            iloot.DestroySelf();
        }
    }
}
