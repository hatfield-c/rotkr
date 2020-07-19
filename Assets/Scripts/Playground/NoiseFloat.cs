using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFloat : MonoBehaviour
{
    public WaterCalculator water;

    void Start()
    {
        
    }

    void Update()
    {
        float newY = water.calculateHeight(
            this.transform.position.x,
            this.transform.position.z
        );

        this.transform.position = new Vector3(
            this.transform.position.x,
            newY,
            this.transform.position.z
        );
    }
}
