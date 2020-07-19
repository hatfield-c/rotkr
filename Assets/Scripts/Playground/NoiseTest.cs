using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTest : MonoBehaviour
{

    public GameObject testObj;
    public GameObject parent;

    protected float time;

    protected Material waterMat;
    protected Texture2D noiseTexture;

    protected float xWidth;
    protected float zWidth;
    protected float halfXWidth;
    protected float halfZWidth;
    protected float originX;
    protected float originZ;

    protected float pixelWidthRatio;
    protected float pixelHeightRatio;

    protected float scrollSpeed;
    protected float zoomLevel;

    protected float vertScale;
    protected float vertOffset;

    void Start()
    {
        MeshFilter mf = this.GetComponent<MeshFilter>();
        this.waterMat = this.GetComponent<Renderer>().material;

        this.halfXWidth = mf.mesh.bounds.extents.x;
        this.halfZWidth = mf.mesh.bounds.extents.z;

        this.xWidth = 2 * this.halfXWidth;
        this.zWidth = 2 * this.halfZWidth;

        this.originX = this.transform.position.x - this.halfXWidth;
        this.originZ = this.transform.position.z - this.halfZWidth;

        this.scrollSpeed = this.waterMat.GetFloat("_Speed");
        this.zoomLevel = this.waterMat.GetFloat("_ZoomLevel");

        this.noiseTexture = (this.waterMat.GetTexture("_NoiseMap") as Texture2D);


        this.pixelWidthRatio = this.noiseTexture.width / this.xWidth;
        this.pixelHeightRatio = this.noiseTexture.height / this.zWidth;

        this.vertScale = this.waterMat.GetFloat("_Scale");
        this.vertOffset = this.waterMat.GetFloat("_VerticalOffset");
    }

    void Update(){
        int childIndex = 0;
        float time = Time.time;

        for(int xi = -5; xi <= 5; xi++){
            for(int zi = -5; zi <= 5; zi++){
                Transform childObject = this.parent.transform.GetChild(childIndex);

                float xCoord = xi - this.originX;
                float zCoord = zi - this.originZ;

                float pixelX = xCoord * this.pixelWidthRatio;
                float pixelZ = zCoord * this.pixelHeightRatio;

                int pixelXMod = (int)Mathf.Floor((pixelX / this.zoomLevel) + (time * this.scrollSpeed * this.noiseTexture.width));
                int pixelZMod = (int)Mathf.Floor((pixelZ / this.zoomLevel) + (time * this.scrollSpeed * this.noiseTexture.height));

                float n = this.noiseTexture.GetPixel(pixelXMod, pixelZMod).r;
                n = (n * this.vertScale) - this.vertOffset;

                childObject.transform.position = new Vector3(-xi, n, -zi);

                childIndex++;
            }
        }
    }

}
