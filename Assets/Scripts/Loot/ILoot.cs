using UnityEngine;

public interface ILoot
{
    void Init(GameObject waterplane);
    GameObject GetGameObject();
    Rigidbody GetRigidbody();
    void DestroySelf();
}
