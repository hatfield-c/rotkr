using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSampler : MonoBehaviour
{
    [SerializeField] Transform target = null;

    protected Vector3 vectorBuffer = new Vector3();

    void Start(){
        this.transform.parent = null;
    }

    void FixedUpdate()
    {
        this.transform.position = this.target.position;

        this.vectorBuffer.y = this.target.eulerAngles.y;
        this.transform.eulerAngles = this.vectorBuffer;
    }

    public Vector3 GetSamplePoint(int index){
        return this.transform.GetChild(index).position;
    }
}
