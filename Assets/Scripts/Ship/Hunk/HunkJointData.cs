using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkJointData
{
    public GameObject origin;
    public float breakForce = 0.0f;
    public float breakTorque = 0.0f;
    public bool jointCollision = false;
    public bool enablePreprocessing = true;
    public float massScale = 1f;
    public float connectedMassScale = 1f;
}
