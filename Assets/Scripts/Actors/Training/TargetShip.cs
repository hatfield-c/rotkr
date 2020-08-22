using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShip : MonoBehaviour
{
    [Header("References")]
    public List<HunkTrain> hunkList = new List<HunkTrain>();
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
    public float waterCutoff = 0.5f;

    protected int speedDir = 1;
    protected float curSpeed;
    protected float speedLerp;

    protected WaterCalculator waterCalculator;
    protected Transform spawnPoints;
    protected Vector3 destination;

    Vector3 horizontalScale = new Vector3(1, 0, 1);
    protected Vector3 forwardBuffer;

    void FixedUpdate(){
        this.UpdateDestination();
        this.TurnToLookAt();
    }

    public void Init(
        ShipAgentTrain agent,
        GameObject waterLevel,
        Transform spawnPoints, 
        List<Collider> colliders
    ){
        this.agent = agent;
        this.waterCalculator = waterLevel.GetComponent<WaterCalculator>();
        this.destinationObject.Init(waterLevel);
        this.buoyancyManager.Init(waterLevel);
        this.spawnPoints = spawnPoints;

        foreach(HunkTrain hunk in this.hunkList){
            hunk.TrainInit(this.agent, this.breakForce);
        }

        this.destinationObject.transform.parent = null;
        this.NewDestination();
        this.IgnoreCollisions(colliders);
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
            this.destination.x - this.transform.position.x,
            this.destination.z - this.transform.position.z
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
        this.destinationObject.transform.position = this.destination;
    }

    public Vector3 choosePosition(){
        int point = Random.Range(0, this.spawnPoints.childCount);

        return this.spawnPoints.GetChild(point).position;
    }

    protected void UpdateDestination(){
        if(Vector3.Distance(this.transform.position, this.destinationObject.transform.position) <= this.targetDistance){
            this.NewDestination();
        }
    }

    protected void IgnoreCollisions(List<Collider> colliders){
        if(colliders == null || colliders.Count < 1){
            return;
        }

        foreach(Collider myCollider in this.myColliders){
            foreach(Collider collider in colliders){
                Physics.IgnoreCollision(myCollider, collider);
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
