using UnityEngine;

public interface ILoot
{
    void Init(GameObject waterplane, LootManager.StorageFunction storageFunction);
    GameObject GetGameObject();
    Rigidbody GetRigidbody();
    void ReturnToPool();
}
