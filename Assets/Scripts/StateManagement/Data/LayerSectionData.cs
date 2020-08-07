using System.Collections.Generic;
/// <summary>
/// Stores all data inside of a LayerSection, holds a list <see cref="WyeData"/> and memory of the player choosing on this section.
/// </summary>
public class LayerSectionData
{
    /// <summary>
    /// Creates a new <see cref="LayerSectionData"/> and populates a list of <see cref="WyeData"/> of size between 1 and <paramref name="branchRange"/> inclusive.
    /// </summary>
    /// <param name="branchRange"></param>
    public LayerSectionData(int branchRange)
    {
        ChosenNodeIndex = 0;
        WyeDatum = new List<WyeData>();

        int randomSectionNodeNumber = UnityEngine.Random.Range(1, (branchRange + 1));
        for (int i = 0; i < randomSectionNodeNumber; i++)
            WyeDatum.Add(new WyeData(true));
    }
    public bool WasChosen;
    public int ChosenNodeIndex;
    public List<WyeData> WyeDatum;

    public void ChooseNode(int chosenNodeIndex = 0)
    {
        WasChosen = true;
        ChosenNodeIndex = chosenNodeIndex;
    }
}