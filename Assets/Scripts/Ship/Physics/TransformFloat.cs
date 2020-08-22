using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFloat : MonoBehaviour
{
    public WaterCalculator water;
    protected Vector3 buffer = new Vector3();

    public void Init(GameObject waterLevel){
        this.water = waterLevel.GetComponent<WaterCalculator>();
    }

    void FixedUpdate()
    {
        float newY = water.calculateHeight(
            this.transform.position.x,
            this.transform.position.z
        );

        this.buffer.x = this.transform.position.x;
        this.buffer.y = newY;
        this.buffer.z = this.transform.position.z;

        this.transform.position = buffer;
    }
}
