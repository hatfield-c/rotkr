using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerSection : MonoBehaviour
{
    #region references
    LayerSectionData layerSectionData;
    GameObject wyeNodePrefab = null;
    List<WyeNode> nodes;
    #endregion

    #region public functions
    public void Init(LayerSectionData layerSectionData, GameObject wyeNodePrefab, ToggleGroup wyeNodeToggleGroup, bool isCurrentSection)
    {
        // store the parameters
        this.layerSectionData = layerSectionData;
        this.wyeNodePrefab = wyeNodePrefab;

        // Create each node and attach it to the section, also pass the data the node needs
        nodes = new List<WyeNode>();
        foreach (WyeData data in layerSectionData.WyeDatum)
        {
            GameObject GO = Instantiate(wyeNodePrefab, transform);
            WyeNode node = GO.GetComponent<WyeNode>();
            node.Init(data);
            node.GetComponent<Toggle>().group = wyeNodeToggleGroup;
            nodes.Add(node);

            // If this section has choices in the past, show them
            if (layerSectionData.WasChosen)
            {
                if (GO.transform.GetSiblingIndex() == layerSectionData.ChosenNodeIndex)
                {
                    node.DisplayHistory(true);
                }
                else
                {
                    node.DisplayHistory(false);
                }
            }
        }

        // Modify the section based on if it's the current, past, and future section
        if (isCurrentSection)
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            foreach (WyeNode node in nodes)
                node.ToggleInteractable(false);
            if (layerSectionData.WasChosen) // past section
            {
                GetComponent<Image>().color = Color.grey;
            }
            else // if future section
            {
            }

        }
    }
    #endregion
}
