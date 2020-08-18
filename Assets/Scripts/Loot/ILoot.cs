using UnityEngine;

public interface ILoot
{
    void Init(GameObject waterplane, Warehouse.StorageFunction storageFunction);
    GameObject GetGameObject();
    Rigidbody GetRigidbody();
    void ReturnToPool();
}
