using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class Ship_Move_Area : MonoBehaviour {
    public Ship_Move_Agent shipAgent;
    public TextMeshPro rewardText;

    public Ship_Move_Target target;

    protected Vector3 desiredPosition;

    protected Rigidbody rb;
    protected float maxRange = 500f;
    protected float resetRange = 10f;

    protected float speed = 0f;//10f;
    public float maxSpeed = 20f;

    void Start(){
        this.rb = this.target.gameObject.GetComponent<Rigidbody>();
        this.ResetArea();
    }

    void Update() {
        this.rewardText.text = this.shipAgent.GetCumulativeReward().ToString();

        this.target.getTransform().LookAt(this.desiredPosition);

        if(this.rb.velocity.magnitude < this.maxSpeed){
            this.rb.AddForce(
                this.target.getTransform().forward * this.speed
            );
        }

        if(
            Vector3.Distance(this.target.getTransform().position, this.desiredPosition) <= this.resetRange
        ) {
            this.randomTarget();
        }
    }

    public void ResetArea(){
        this.target.resetSelf();

        this.target.getTransform().position = this.getRandomPosition();

        this.randomTarget();
    }

    public void randomTarget() {
        this.desiredPosition = this.getRandomPosition();

        this.target.getTransform().LookAt(this.desiredPosition);
    }

    public Vector3 getRandomPosition(){
        float ranX = Random.Range(50, 500);
        float ranZ = Random.Range(50, 500);

        float dirX = 0;
        float dirZ = 0;

        float dice = Random.Range(0f, 1f);

        if(dice <= 0.5f)
            dirX = 1;
        else
            dirX = -1;

        dice = Random.Range(0f, 1f);

        if(dice <= 0.5f)
            dirZ = 1;
        else
            dirZ = -1;

        return new Vector3(
            dirX * ranX,
            -4.43f,
            dirZ * ranZ
        ) + this.GetComponent<Transform>().position;
    }

    public Rigidbody getRb(){
        return this.rb;
    }
}
