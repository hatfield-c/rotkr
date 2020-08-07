using System.Collections.Generic;

public class GameProgressionData
{
    public GameProgressionData(int layerCount, int layerSectionCount, int layerBranchRange)
    {
        // Record parameters.
        LayerCount = layerCount;
        LayerSectionCount = layerSectionCount;
        LayerBranchRange = layerBranchRange;

        LayerMapDatum = new List<LayerMapData>();

        // Create the first LayerMap.
        LayerMapDatum.Add(new LayerMapData(LayerSectionCount, LayerBranchRange));

        // Create the player's ShipData.
        ShipData = new ShipData();
    }

    public int LayerCount;
    public int LayerSectionCount;
    public int LayerBranchRange;
    public int CurrentLayerIndex;
    public List<LayerMapData> LayerMapDatum;
    public ShipData ShipData;

    /// <summary>
    /// Updates the <see cref="GameProgressionData"/> after completing a section.
    /// </summary>
    /// <param name="wyeData"></param>
    /// <returns>true if the next section is the last section in the game, false otherwise</returns>
    public bool CompleteSection(WyeData wyeData)
    {
        LayerMapData layer = LayerMapDatum[CurrentLayerIndex];

        // Update Section Data class
        LayerSectionData section = layer.LayerSectionDatum[layer.CurrentSectionIndex];
        for (int i = 0; i < section.WyeDatum.Count; i++)
        {
            if (section.WyeDatum[i].ID == wyeData.ID)
            {
                section.ChooseNode(i);
                break;
            }
        }

        // Update Layer Data class
        layer.CurrentSectionIndex++;

        if (layer.CurrentSectionIndex >= layer.SectionCount)
            CompleteLayer(layer);

        if (CurrentLayerIndex >= LayerCount - 1 && layer.CurrentSectionIndex >= LayerSectionCount)
            return true;
        else
            return false;
    }

    void CompleteLayer(LayerMapData layer)
    {
        layer.CurrentSectionIndex = layer.SectionCount;
        layer.Completed = true;
        CurrentLayerIndex++;

        if (CurrentLayerIndex >= LayerCount)
        {
            CurrentLayerIndex = LayerCount;
        }
        else
        {
            LayerMapDatum.Add(new LayerMapData(LayerSectionCount, LayerBranchRange));
        }
    }
}