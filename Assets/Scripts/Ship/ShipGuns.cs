using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGuns : MonoBehaviour
{
    public float ShootForce = 1000;
    public GameObject bulletMat;
    public Transform[] guns;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < this.guns.Length; i++){
            if(Input.GetKeyDown(KeyCode.Space)){
                Vector3 dist = this.guns[i].transform.up * 1.5f;
                GameObject bullet = Instantiate(
                    this.bulletMat, 
                    this.guns[i].position + dist, 
                    Quaternion.identity
                );

                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                Vector3 shootDir = this.guns[i].up;
                
                bulletRb.AddForce(shootDir * ShootForce);
            }
        }


    }
}
