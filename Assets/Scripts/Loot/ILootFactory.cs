using UnityEngine;
using System.Collections.Generic;

public interface ILootFactory
{
    void Init(GameObject waterplane);
    List<ILoot> CreateLoot();
}
