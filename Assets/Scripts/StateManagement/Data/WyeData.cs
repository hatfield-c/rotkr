using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store and load data in <see cref="WyeState"/>.
/// </summary>
public class WyeData
{
    /// <summary>
    /// Creates a new <see cref="WyeData"/> of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    public WyeData(TypeOfWye type)
    {
        AssignID();
        WyeType = type;
    }

    /// <summary>
    /// Creates a new <see cref="WyeData"/> with random attributes if <paramref name="createRandom"/> is true.
    /// </summary>
    /// <param name="createRandom"></param>
    public WyeData(bool createRandom)
    {
        AssignID();
        if (createRandom)
        {
            WyeType = TypeOfWye.CollectionChamber;
            if (Random.Range(0, 1f) > .5f)
                WyeType = TypeOfWye.Spillway;
        }
    }

    void AssignID()
    {
        if (IDs == null)
            IDs = new List<int>();

        ID = IDs.Count;
        IDs.Add(ID);
    }
    public TypeOfWye WyeType;
    public int ID;
    public static List<int> IDs;
}