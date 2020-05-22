using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float AirDrag = 1;
    public float WaterDrag = 5;
    public bool AffectDirection = true;
    public bool AttachToSurface = false;
    public Transform[] FloatPoints;

    protected Rigidbody rb;
    protected Water water;

    protected float waterLine;
    protected Vector3[] waterLinePoints;

    protected Vector3 centerOffset;
    protected Vector3 smoothVectorRotation;
    protected Vector3 TargetUp;

    public Vector3 Center { get { return this.transform.position + centerOffset; } }

    // Start is called before the first frame update
    void Awake()
    {
        this.water = FindObjectOfType<Water>();
        this.rb = GetComponent<Rigidbody>();
        this.rb.useGravity = false;

        this.waterLinePoints = new Vector3[this.FloatPoints.Length];
        for(int i = 0; i < this.FloatPoints.Length; i++){
            this.waterLinePoints[i] = this.FloatPoints[i].position;
        }
        this.centerOffset = PhysicsHelper.GetCenter(this.waterLinePoints) - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var newWaterLine = 0f;
        var pointUnderWater = false;

        for(int i = 0; i < this.FloatPoints.Length; i++){
            this.waterLinePoints[i] = this.FloatPoints[i].position;
            this.waterLinePoints[i].y = this.water.GetHeight(this.FloatPoints[i].position);
            newWaterLine += this.waterLinePoints[i].y / this.FloatPoints.Length;

            if(this.waterLinePoints[i].y > this.FloatPoints[i].position.y){
                pointUnderWater = true;
            }
        }

        var waterLineDelta = newWaterLine - this.waterLine;
        this.waterLine = newWaterLine;

        var gravity = Physics.gravity;
        this.rb.drag = this.AirDrag;

        if(this.waterLine > this.Center.y){
            this.rb.drag = this.WaterDrag;

            if(this.AttachToSurface){
                this.rb.position = new Vector3(this.rb.position.x, this.waterLine - this.centerOffset.y, this.rb.position.z);
            } else {
                gravity = -gravity;
                this.transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        this.rb.AddForce(gravity * Mathf.Clamp(Mathf.Abs(this.waterLine - Center.y), 0, 1));

        this.TargetUp = PhysicsHelper.GetNormal(this.waterLinePoints);

        if(pointUnderWater){
            this.TargetUp = Vector3.SmoothDamp(this.transform.up, this.TargetUp, ref smoothVectorRotation, 0.2f);
            this.rb.rotation = Quaternion.FromToRotation(this.transform.up, this.TargetUp) * this.rb.rotation;
        }
    }
}
