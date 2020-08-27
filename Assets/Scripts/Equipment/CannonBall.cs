using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] Rigidbody physicsBody = null;
    public GameObject owner;
    public float despawnTime = 7f;

    protected KatinTimer despawnTimer = new KatinTimer();

    void FixedUpdate(){
        this.despawnTimer.update();
    }

    public void activate(){
        this.gameObject.SetActive(true);
        this.despawnTimer.Init(this.despawnTime, this.deactivate);
    }

    public void deactivate(){
        this.physicsBody.velocity = Vector3.zero;
        this.physicsBody.angularVelocity = Vector3.zero;
        this.transform.eulerAngles = Vector3.zero;

        this.despawnTimer.disable();
        this.gameObject.SetActive(false);
    }

    public Rigidbody getRigidbody(){
        return this.physicsBody;
    }
}
