using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShip : MonoBehaviour
{
    [Header("References")]
    public List<HunkTrain> hunkList = new List<HunkTrain>();
    public List<TargetRat> ratList = new List<TargetRat>();
    public Rigidbody rb;
    public TransformFloat destinationObject;
    public BuoyancyManager buoyancyManager;
    public List<Collider> myColliders = new List<Collider>();
    public ShipAgentTrain agent;

    [Header("Speed Parameters")]
    public bool variedSpeed = false;
    public float speedMod = 500f;
    public float maxSpeed = 50f;
    public float minSpeed = 10f;
    public float power = 50f;
    public float flipModifier = 6f;
    public float forwardBias = 0.1f;
    public float angleThreshold = 30f;

    [Header("Turn Parameters")]
    public float maxTurn = 20000f;
    public float minTurn = 10000f;
    public float oppositeChance = 0.3f;

    [Header("Parameters")]
    public float targetDistance = 3f;
    public float breakForce = 3000f;
    public float ratBreakForce = 10f;
    public float waterCutoff = 0.5f;

    protected int speedDir = 1;
    protected float curSpeed;
    protected float speedLerp;

    protected WaterCalculator waterCalculator;
    protected Transform spawnPoints;
    protected Transform destination;

    Vector3 horizontalScale = new Vector3(1, 0, 1);
    protected Vector3 forwardBuffer;
    protected List<Collider> terrainColliders;

    void FixedUpdate(){
        this.UpdateDestination();
        this.TurnToLookAt();
    }

    public void Init(
        ShipAgentTrain agent,
        GameObject waterLevel,
        Transform spawnPoints
    ){
        this.agent = agent;
        this.waterCalculator = waterLevel.GetComponent<WaterCalculator>();
        this.destinationObject.Init(waterLevel);
        this.buoyancyManager.Init(waterLevel);
        this.spawnPoints = spawnPoints;

        foreach(HunkTrain hunk in this.hunkList){
            hunk.TrainInit(this.agent, this.breakForce, this.hunkList.Count);
        }

        foreach(TargetRat rat in this.ratList){
            rat.TrainInit(this.agent, this.ratBreakForce, this.ratList.Count);
        }

        this.transform.eulerAngles = new Vector3(
            0,
            Random.Range(0f, 360f),
            0
        );

        this.destinationObject.transform.parent = null;
    }

    public void Reset(List<Collider> colliders = null){
        foreach(HunkTrain hunk in this.hunkList){
            hunk.Reset();
        }

        foreach(TargetRat rat in this.ratList) {
            rat.Reset();
        }

        this.rb.velocity = Vector3.zero;
        this.transform.eulerAngles = new Vector3(
            0,
            Random.Range(0f, 360f),
            0
        );

        this.NewDestination();

        this.IgnoreCollisions(false);
        this.terrainColliders = colliders;
        this.IgnoreCollisions(true);
    }

    public void AddSpeed(){

        if(CanAccelerate()){
            this.forwardBuffer = Vector3.Scale(this.horizontalScale, this.transform.forward);
        } else {
            return;
        }

        this.curSpeed = this.rb.velocity.magnitude;
        this.curSpeed = Mathf.Clamp(this.curSpeed, this.minSpeed, this.maxSpeed);

        if(this.speedDir > 0){
            this.speedLerp = Mathf.InverseLerp(this.minSpeed, this.maxSpeed, this.curSpeed);
        } else {
            this.speedLerp = Mathf.InverseLerp(-this.maxSpeed, -this.minSpeed, -this.curSpeed);
        }

        if(Random.value < Mathf.Pow(
                this.speedLerp, 
                this.flipModifier
            )
        ){
            if(this.speedDir < 0){
                this.speedDir = 1;
            } else {
                if(Random.value < this.forwardBias){
                    this.speedDir = -1;
                }
            }
        }

        PhysicsHelper.ApplyForceToReachVelocity(this.rb, forwardBuffer * this.speedMod, this.power * this.speedDir);
    }

    public void TurnToLookAt(){
        Vector2 targetDirection = new Vector2(
            this.destination.position.x - this.transform.position.x,
            this.destination.position.z - this.transform.position.z
        );

        float angle = Vector2.SignedAngle(
            targetDirection, 
            new Vector2(
                this.transform.forward.x,
                this.transform.forward.z
            )
        );

        if(angle < 0.0000001 && angle > -0.0000001){
            return;
        }

        float direction = angle / Mathf.Abs(angle);
        float amount = Random.Range(this.minTurn, this.maxTurn);

        if(Random.value < this.oppositeChance){
            direction *= -1;
        }

        this.rb.AddTorque(this.transform.up * direction * amount);

        if(Mathf.Abs(angle) <= this.angleThreshold){
            this.AddSpeed();
        }
    }

    public void NewDestination(){
        this.destination = this.choosePosition();
    }

    public Transform choosePosition(){
        int point = Random.Range(0, this.spawnPoints.childCount);

        return this.spawnPoints.GetChild(point);
    }

    protected void UpdateDestination(){
        if(Vector3.Distance(this.transform.position, this.destinationObject.transform.position) <= this.targetDistance){
            this.NewDestination();
        }

        this.destinationObject.transform.position = this.destination.position;
    }

    protected void IgnoreCollisions(bool ignore){
        if(this.terrainColliders == null || this.terrainColliders.Count < 1){
            return;
        }

        foreach(Collider myCollider in this.myColliders){
            foreach(Collider collider in this.terrainColliders){
                Physics.IgnoreCollision(myCollider, collider, ignore);
            }
        }
    }

    protected bool CanAccelerate(){
        if(this.waterCalculator == null){
            return true;
        }

        float height = this.waterCalculator.calculateHeight(
            this.transform.position.x,
            this.transform.position.z
        );

        if(this.transform.position.y > height + waterCutoff){
            return false;
        }

        return true;
    }
    
}
