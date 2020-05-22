using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public const int MAX_X = 5;
    public const int MAX_Z = 10;
    public const int MAX_Y = 4;

    public TextAsset bluePrintFile;
    public GameObject coreMat;
    public GameObject buildMat;

    protected string[,,] bluePrint = new string[MAX_X, MAX_Z, MAX_Y];
    protected GameObject[,,] bloks = new GameObject[MAX_X, MAX_Z, MAX_Y];
    protected Vector3 corePos = new Vector3(-1, -1, -1);

    // Start is called before the first frame update
    void Start()
    {
        this.loadBluePrint();

        if(this.corePos.x < 0){
            Debug.LogWarning("WARNING: No valid blueprint core detected!");
            return;
        }

        this.buildShip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void buildShip(){
        GameObject core = Instantiate(
            this.coreMat, 
            this.transform.position, 
            Quaternion.identity
        );
        core.transform.parent = this.transform;
        this.bloks[(int)this.corePos.x, (int)this.corePos.z, (int)this.corePos.y] = core;

        for(int yI = 0; yI < MAX_Y; yI++){
            for(int xI = 0; xI < MAX_X; xI++){
                for(int zI = 0; zI < MAX_Z; zI++){
                    if(yI == this.corePos.y && xI == this.corePos.x && zI == this.corePos.z){
                        continue;
                    }

                    if(this.bluePrint[xI, zI, yI] == "0"){
                        continue;
                    }

                    Vector3 dir = (new Vector3(xI, yI, zI)) - this.corePos;

                    GameObject blok = Instantiate(
                        this.buildMat, 
                        this.transform.position + (dir * core.transform.localScale.x), 
                        Quaternion.identity
                    );
                    blok.transform.parent = this.transform;

                    this.bloks[xI, zI, yI] = blok;
                }
            }
        }

    }

    protected void loadBluePrint(){
        string rawPrint = this.bluePrintFile.ToString();

        int yI = 0;
        string[] yAxis = rawPrint.Split(new string[1] {":::"}, System.StringSplitOptions.None);
        foreach(string yRaw in yAxis){
            string y = yRaw.Trim();
            string[] xAxis = y.Split(new string[1] {System.Environment.NewLine}, System.StringSplitOptions.None);

            int xI = 0;
            foreach(string xRaw in xAxis){
                string x = xRaw.Trim();
                string[] zAxis = x.Split(',');

                int zI = 0;
                foreach(string zRaw in zAxis){
                    string z = zRaw.Trim();

                    if(z == "X"){
                        this.corePos = new Vector3(xI, yI, zI);
                    }

                    this.bluePrint[xI, zI, yI] = z;

                    zI++;
                }

                xI++;
            }

            yI++;
        }
    }
}
