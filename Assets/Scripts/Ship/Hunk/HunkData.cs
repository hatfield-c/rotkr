using UnityEngine;

public class HunkData {
    public int HunkID;
    public bool Deleted;

    public HunkData(
        int hunkID, 
        bool deleted
    ) {
        this.HunkID = hunkID;
        this.Deleted = deleted;
    }
}
