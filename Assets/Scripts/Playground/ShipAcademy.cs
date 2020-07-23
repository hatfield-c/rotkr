using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class ShipAcademy : MonoBehaviour {
    public void Awake(){
        Academy.Instance.OnEnvironmentReset += this.ResetEnv;
    }

    public void ResetEnv(){

    }
}
