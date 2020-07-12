using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float despawnTime = 7f;

    protected KatinTimer despawnTimer;
    protected Rigidbody physicsBody;

    void Awake()
    {
        this.physicsBody = this.GetComponent<Rigidbody>();
        this.despawnTimer = new KatinTimer();
    }

    void FixedUpdate(){
        this.despawnTimer.update();
    }

    public void activate(){
        this.gameObject.SetActive(true);
        this.despawnTimer.Init(this.despawnTime, this.deactivate);
    }

    public void deactivate(){
        this.physicsBody.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    public Rigidbody getRigidbody(){
        return this.physicsBody;
    }
}
