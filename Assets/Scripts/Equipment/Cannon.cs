using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, IEquipment
{
    public CannonBall projectile;
    public Transform shootOrigin;
    public float projectileAngle = 2.736f;
    public float projectileAcceleration = 10f;
    public float reloadTime = 1.5f;
    public float BaseReloadTime = 1.5f;
    public float maxDelay = 0.5f;
    public float minDelay = 0f;

    protected bool loaded = true;
    protected KatinTimer fuseTimer;
    protected KatinTimer reloadTimer = new KatinTimer();
    protected InputMaster controls = null;

    void OnEnable() {
        if(controls != null)
            controls.Player.Shoot.performed += context => this.lightFuse();
    }

    void OnDisable() {
        if (controls != null)
            controls.Player.Shoot.performed -= context => this.lightFuse();
    }

    void Start() {
        this.fuseTimer = new KatinTimer();
        this.reloadTimer = new KatinTimer();
        this.shootOrigin.eulerAngles = new Vector3(
            -this.projectileAngle,
            this.shootOrigin.eulerAngles.y,
            this.shootOrigin.eulerAngles.z
        );
    }

    void Update() {
        this.fuseTimer.update();
        this.reloadTimer.update();
    }

    public void fire(){
        this.projectile.activate();

        this.projectile.getRigidbody().velocity = Vector3.zero;
        this.projectile.transform.position = this.shootOrigin.position;
        this.projectile.transform.rotation = this.shootOrigin.rotation;

        this.projectile.getRigidbody().AddForce(
            this.projectile.transform.forward * this.projectileAcceleration,
            ForceMode.VelocityChange
        );
    }

    public void lightFuse(){
        if(!this.loaded){
            return;
        }

        float fuseTime = Random.Range(this.minDelay, this.maxDelay);
        this.fuseTimer.Init(fuseTime, this.fire);
        this.reloadTimer.Init(this.reloadTime, this.reload);

        this.loaded = false;
    }

    public void reload(){
        this.loaded = true;
    }

    public void registerInput(InputMaster controls){
        this.controls = controls;
        controls.Player.Shoot.performed += context => this.lightFuse();
    }
    public void UpdateReloadSpeed(float newReloadRatio)
    {
        reloadTime = BaseReloadTime * newReloadRatio;
    }
}
