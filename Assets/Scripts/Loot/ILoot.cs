using UnityEngine;

public interface ILoot
{
    GameObject GetGameObject();
    Rigidbody GetRigidbody();
    void DestroySelf();
}
