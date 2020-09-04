using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    [SerializeField] Transform hunkGroup = null;
    public float BaseBreakForce = 3000f;
    public HunkJointData jointData;
    public HunkRigidbodyData rigidBodyData;
    public float hunkDespawnTime = 7f;
    public Action HunkBroken;

    protected Transform origParent;
    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkDatum){
        this.hunkList = buildFromData(hunkDatum);

        this.origParent = this.hunkGroup.parent;
        this.hunkGroup.position = this.origParent.position;
        this.hunkGroup.rotation = this.origParent.rotation;
        this.hunkGroup.parent = null;
    }

    public List<Hunk> GetDeletedHunks(){
        List<Hunk> hunks = new List<Hunk>();

        foreach(Hunk hunk in this.hunkList){
            if(hunk.IsDeleted()){
                hunks.Add(hunk);
            }
        }

        return hunks;
    }

    public int GetHunkCount(){
        return this.hunkList.Count;
    }

    public void EnableHunks(){
        this.hunkGroup.gameObject.SetActive(true);
        this.hunkGroup.position = this.origParent.position;
        this.hunkGroup.rotation = this.origParent.rotation;
        this.hunkGroup.parent = null;

        foreach(Hunk hunk in this.hunkList){
            hunk.EnableHunk();
        }
    }

    public void DisableHunks(){
        foreach(Hunk hunk in this.hunkList){
            hunk.DisableHunk();
        }

        this.hunkGroup.parent = this.origParent;
        this.hunkGroup.gameObject.SetActive(false);
    }

    protected List<Hunk> buildFromData(List<HunkData> hunkDatum) {
        List<Hunk> hunks = new List<Hunk>();

        if(hunkDatum.Count > 0) {
            foreach (HunkData data in hunkDatum) {
                Hunk hunk = this.hunkGroup.GetChild(data.HunkID).GetComponent<Hunk>();
                
                hunk.Init(
                    data, 
                    this.jointData, 
                    this.rigidBodyData,
                    this.HunkBroken,
                    this.hunkDespawnTime
                );

                hunks.Add(hunk);
            }
        }
        else {
            for(int i = 0; i < hunkGroup.childCount; i++) {
                Hunk hunk = this.hunkGroup.GetChild(i).GetComponent<Hunk>();
                HunkData data = new HunkData(i, false);

                hunk.Init(
                    data, 
                    this.jointData, 
                    this.rigidBodyData, 
                    this.HunkBroken,
                    this.hunkDespawnTime
                );

                hunks.Add(hunk);
                hunkDatum.Add(data);
            }
        }
        return hunks;
    }

    public void UpdateHunkBreakForce(float newBreakForce)
    {
        jointData.breakForce = newBreakForce;
    }

}
