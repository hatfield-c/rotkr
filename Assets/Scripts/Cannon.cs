using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject barrel;
    public GameObject barrelEnd;
    public GameObject bulletMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            GameObject bullet = Instantiate(
                this.bulletMat, 
                this.barrelEnd.transform.position, 
                Quaternion.identity
            );

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            Vector3 shootDir = this.barrel.transform.up;
            
            bulletRb.AddForce(shootDir * 1000);
        }
    }
}
