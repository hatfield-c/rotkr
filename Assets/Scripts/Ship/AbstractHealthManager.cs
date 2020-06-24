using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHealthManager : MonoBehaviour
{
    public HunkManager hunkManager;

    public void Start(){
        this.hunkManager.Init(null);
    }

}
