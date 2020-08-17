using UnityEngine;

public interface IStorable
{
    GameObject GetGameObject();
    string GetArchetype();
    void Enable();
    void Disable();
}
