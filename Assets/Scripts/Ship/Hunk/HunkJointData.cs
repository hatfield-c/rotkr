using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkJointData
{
    public GameObject origin;
    public float breakForce = 15000.0f;
    public float breakTorque = Mathf.Infinity;
    public bool jointCollision = false;
    public bool enablePreprocessing = false;
    public float massScale = 1f;
    public float connectedMassScale = .00001f;
}
