using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[System.Serializable]
public class OverboardSwimmer
{
    public Action Rescued;

    [SerializeField] float pickupDelay = 3;

    protected GameObject assignedDeck = null;
    protected GameObject aoeObject = null;
    protected Sequence aoeSequence = null;

    public delegate bool IsAttachedDelegate();

    public void Init(ShipReferences shipReferences, RatReferences ratReferences, IsAttachedDelegate isAttached){
        this.assignedDeck = shipReferences.DeckObject;
        this.aoeObject = ratReferences.AoeObject;

        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        sequence.OnComplete(this.ResetTween);
        sequence.InsertCallback(this.pickupDelay, () => {
            if (!isAttached()){
                this.AllowPickup();
            }
        });
        this.aoeSequence = sequence;
    }

    public void AttachActivate(){
        this.aoeObject.SetActive(false);
        Rescued?.Invoke();
    }

    public void OverboardActivate(){
        this.aoeSequence.Play();
    }

    public void AllowPickup(){
        this.aoeObject.SetActive(true);
    }

    public void TriggerActivate(Collider collider){
        if(collider.gameObject != this.assignedDeck){
            return;
        }

        this.AttachActivate();
    }

    void ResetTween(){
        this.aoeSequence.Restart();
        this.aoeSequence.Pause();
    }

    void OnDestroy() {
        this.aoeSequence.Kill();
    }
}
