using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASensor : MonoBehaviour
{
    [SerializeField] protected bool debug;
    [SerializeField] List<string> tagList = null;
    [SerializeField] int id = 0;
    [SerializeField] protected int sensorCount;
    [SerializeField] protected float distance;
    [SerializeField] protected float sphereRadius;

    protected Vector3 startBuffer = new Vector3();
    protected Vector3 dirBuffer = new Vector3();
    protected RaycastHit hitBuffer = new RaycastHit();

    protected List<bool> results = new List<bool>();

    public abstract List<bool> ReadSensors();

    public float GetSensorCount() {
        return this.sensorCount;
    }

    public float GetId() {
        return this.id;
    }

    protected bool ReadSensor(Vector3 origin, Vector3 direction) {
        bool hasHit = Physics.SphereCast(
            origin, 
            this.sphereRadius,
            direction,
            out this.hitBuffer,
            this.distance
        );

        if (!hasHit) {
            return false;
        }

        return this.IsReadableTag(this.hitBuffer.collider.tag);
    }

    protected bool IsReadableTag(string tag) {
        return this.tagList.Contains(tag);
    }
}
