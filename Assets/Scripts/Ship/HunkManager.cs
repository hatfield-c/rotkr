using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    [SerializeField] Transform hunkGroup = null;
    public HunkJointData jointData;
    public HunkRigidbodyData rigidBodyData;
    public float hunkDespawnTime = 7f;

    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkDatum) {
        this.hunkGroup.parent = null;

        this.hunkList = buildFromData(hunkDatum);
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

    protected List<Hunk> buildFromData(List<HunkData> hunkDatum) {
        List<Hunk> hunks = new List<Hunk>();

        if(hunkDatum.Count > 0) {
            foreach (HunkData data in hunkDatum) {
                Hunk hunk = this.hunkGroup.GetChild(data.HunkID).GetComponent<Hunk>();
                hunk.Init(data, this.jointData, this.rigidBodyData, this.hunkDespawnTime);
                hunks.Add(hunk);
            }
        }
        else {
            for(int i = 0; i < hunkGroup.childCount; i++) {
                Hunk hunk = this.hunkGroup.GetChild(i).GetComponent<Hunk>();
                HunkData data = new HunkData(i, false);

                hunk.Init(data, this.jointData, this.rigidBodyData, this.hunkDespawnTime);
                hunks.Add(hunk);
                hunkDatum.Add(data);
            }
        }
        return hunks;
    }

}
