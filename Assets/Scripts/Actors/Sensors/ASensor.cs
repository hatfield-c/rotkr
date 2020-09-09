using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASensor : MonoBehaviour
{
    [SerializeField] protected bool debug = false;
    [SerializeField] protected bool returnZero = false;
    [SerializeField] List<string> tagList = null;
    [SerializeField] int id = 0;
    [SerializeField] protected int sensorCount;
    [SerializeField] protected int zones = 1;
    [SerializeField] protected float distance;
    [SerializeField] protected float sphereRadius;

    protected Vector3 startBuffer = new Vector3();
    protected Vector3 dirBuffer = new Vector3();
    protected RaycastHit hitBuffer = new RaycastHit();

    protected List<float> results = new List<float>();

    public abstract Vector3 GetStartPos(int rayIndex);

    public abstract Vector3 GetDir(int rayIndex);

    public List<float> ReadSensors() {
        if (this.sensorCount < 1) {
            return this.results;
        }

        this.results.Clear();

        for (int i = 0; i < this.sensorCount; i++) {
            this.startBuffer = this.GetStartPos(i);

            this.dirBuffer = this.GetDir(i);

            int hitZone = this.ReadSensor(
                this.startBuffer,
                this.dirBuffer
            );

            this.results.Add((float)hitZone / this.zones);

            if (this.debug && hitZone > 0) {
                Debug.Log($"Sensor ID {this.GetId()} hit ray {i} in zone {hitZone}.");
            }
        }

        return this.results;
    }

    public float ReadSensorPool() {
        float closestZone = this.zones;

        for (int i = 0; i < this.sensorCount; i++) {
            this.startBuffer = this.GetStartPos(i);

            this.dirBuffer = this.GetDir(i);

            int hitZone = this.ReadSensor(
                this.startBuffer,
                this.dirBuffer
            );

            if (this.debug && hitZone < this.zones) {
                Debug.Log($"Sensor ID {this.GetId()} hit ray {i} in zone {hitZone}.");
            }

            if (hitZone < closestZone) {
                closestZone = hitZone;
            }
        }

        return closestZone / this.zones;
    }

    public float GetSensorCount() {
        return this.sensorCount;
    }

    public float GetId() {
        return this.id;
    }

    protected int ReadSensor(Vector3 origin, Vector3 direction) {
        bool hasHit = Physics.SphereCast(
            origin, 
            this.sphereRadius,
            direction,
            out this.hitBuffer,
            this.distance
        );

        if (!hasHit || !this.IsReadableTag(this.hitBuffer.collider.tag)) {
            return this.zones;
        }

        float zone = ( this.hitBuffer.distance / (this.distance / this.zones) );
        int result = Mathf.CeilToInt(zone);

        return result * (this.returnZero ? 0 : 1);
    }

    protected bool IsReadableTag(string tag) {
        return this.tagList.Contains(tag);
    }

    void Awake() {
        if (debug) {
            GameObject rayObject;
            BoxCollider collider;

            for (int i = 0; i < this.sensorCount; i++) {
                this.startBuffer = this.GetStartPos(i);

                this.dirBuffer = this.GetDir(i);

                rayObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rayObject.transform.localScale = new Vector3(0.1f, 0.1f, this.distance);

                collider = rayObject.GetComponent<BoxCollider>();
                Destroy(collider);

                rayObject.transform.position = this.startBuffer;
                rayObject.transform.LookAt(this.dirBuffer * this.distance + this.startBuffer);
                rayObject.transform.Translate(Vector3.forward * (this.distance / 2));
                rayObject.transform.parent = this.transform;
                rayObject.name = $"DebugRay{i}";
            }
        }
    }
}
