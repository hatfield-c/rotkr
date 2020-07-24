using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <see cref="LayerMapData"/>'s are used to store all data in the <see cref="LayerMapState"/>
/// </summary>
public class LayerMapData
{
    /// <summary>
    /// Create a new <see cref="LayerMapData"/> of size <paramref name="sectionCount"/> with the middle sections having <paramref name="branchRange"/> nodes.
    /// </summary>
    /// <param name="sectionCount"></param>
    /// <param name="branchRange"></param>
    public LayerMapData(int sectionCount, int branchRange)
    {
        // record parameters
        CurrentSectionIndex = 0;
        SectionCount = sectionCount;
        BranchRange = branchRange;

        // The first and last section will have only 1 node.
        if (sectionCount < 2)
        {
            Debug.LogError("Not enough Sections for this LayerMap, exiting");
            return;
        }

        // Initialize our Datum
        LayerSectionDatum = new List<LayerSectionData>();

        for (int i = 0; i < sectionCount; i++)
        {
            // If this is the first or last section in the map, make 1 node no branches
            if (i == 0 || i == sectionCount - 1)
            {
                LayerSectionData newSoloSection = new LayerSectionData(1);
                LayerSectionDatum.Add(newSoloSection);
                continue;
            }

            // Otherwise create some nodes for this section, given it is valid with the remaining nodes
            LayerSectionData newSection = new LayerSectionData(branchRange);
            LayerSectionDatum.Add(newSection);
        }
    }
    public int CurrentSectionIndex;
    public List<LayerSectionData> LayerSectionDatum;
    public int SectionCount;
    public int BranchRange;
    public bool Completed;
}