using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunkData {
    public int hunkId;
    public bool deleted;

    public HunkData(int hunkId, bool deleted){
        this.hunkId = hunkId;
        this.deleted = deleted;
    }
}
