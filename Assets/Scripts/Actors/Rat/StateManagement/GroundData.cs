using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData
{
    bool isGrounded;
    GameObject deckUnderfoot;

    public GroundData(bool isGrounded, GameObject deckUnderfoot){
        this.isGrounded = isGrounded;
        this.deckUnderfoot = deckUnderfoot;
    }

    public bool IsGrounded(){
        return this.isGrounded;
    }

    public GameObject GetDeck(){
        return this.deckUnderfoot;
    }
}
