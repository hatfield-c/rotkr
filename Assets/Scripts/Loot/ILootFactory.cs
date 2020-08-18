using UnityEngine;
using System.Collections.Generic;

public interface ILootFactory
{
    void Init(Warehouse lootWarehouse);
    List<ILoot> GetLoot();
}
