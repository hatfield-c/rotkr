using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public int Dimensions;
    public Octave[] octaves;
    public float UVScale;

    protected MeshFilter meshFilter;
    protected Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        this.mesh = new Mesh();
        this.mesh.name = this.gameObject.name;

        this.mesh.vertices = this.GenerateVerts();
        this.mesh.triangles = this.GenerateTris();
        this.mesh.uv = this.GenerateUVs();
        this.mesh.RecalculateBounds();
        this.mesh.RecalculateNormals();

        this.meshFilter = this.gameObject.AddComponent<MeshFilter>();
        this.meshFilter.mesh = this.mesh;
    }

    // Update is called once per frame
    void Update()
    {
        var verts = this.mesh.vertices;
        for(int x = 0; x <= this.Dimensions; x++){
            for(int z = 0; z <= this.Dimensions; z++){

                var y = 0f;
                for(int oct = 0; oct < this.octaves.Length; oct++){
                    if(this.octaves[oct].alternate){
                        var perlNoise = Mathf.PerlinNoise((x * this.octaves[oct].scale.x) / this.Dimensions, (z * this.octaves[oct].scale.y) / this.Dimensions) * Mathf.PI * 2f;
                        y += Mathf.Cos(perlNoise + this.octaves[oct].speed.magnitude * Time.time) * this.octaves[oct].height;
                    } else {
                        var perlNoise = Mathf.PerlinNoise((x * this.octaves[oct].scale.x + Time.time * this.octaves[oct].speed.x) / this.Dimensions, (z * this.octaves[oct].scale.y + Time.time * this.octaves[oct].speed.y) / this.Dimensions) - 0.5f;
                        y += perlNoise * this.octaves[oct].height;
                    }
                }

                verts[this.index(x, z)] = new Vector3(x, y, z);
            }
        }

        this.mesh.vertices = verts;
        this.mesh.RecalculateNormals();
    }

    public float GetHeight(Vector3 position){
        var scale = new Vector3(1 / this.transform.lossyScale.x, 0, 1 / this.transform.lossyScale.z);
        var localPos = Vector3.Scale((position - this.transform.position), scale);

        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        p1.x = Mathf.Clamp(p1.x, 0, this.Dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, this.Dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, this.Dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, this.Dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, this.Dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, this.Dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, this.Dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, this.Dimensions);

        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                + (max - Vector3.Distance(p2, localPos))
                + (max - Vector3.Distance(p3, localPos))
                + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        var height = this.mesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
                    + this.mesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
                    + this.mesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
                    + this.mesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        return height * this.transform.lossyScale.y / dist;
    }

    private Vector2[] GenerateUVs(){
        var uvs = new Vector2[this.mesh.vertices.Length];

        for(int x = 0; x <= this.Dimensions; x++){
            for(int z = 0; z <= this.Dimensions; z++){
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private Vector3[] GenerateVerts(){
        var verts = new Vector3[(this.Dimensions + 1) * (this.Dimensions + 1)];

        for(int x = 0; x <= this.Dimensions; x++){
            for(int z = 0; z <= this.Dimensions; z++){
                //Debug.Log(x + ", " + z + ": " + verts.Length);
                verts[this.index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    private int index(int x, int z){
        return x * (this.Dimensions + 1) + z;
    }

    private int[] GenerateTris(){
        var tris = new int[this.mesh.vertices.Length * 6];

        for(int x = 0; x < this.Dimensions; x++){
            for(int z = 0; z < this.Dimensions; z++){
                tris[this.index(x, z) * 6 + 0] = this.index(x, z);
                tris[this.index(x, z) * 6 + 1] = this.index(x + 1, z + 1);
                tris[this.index(x, z) * 6 + 2] = this.index(x + 1, z);
                tris[this.index(x, z) * 6 + 3] = this.index(x, z);
                tris[this.index(x, z) * 6 + 4] = this.index(x, z + 1);
                tris[this.index(x, z) * 6 + 5] = this.index(x + 1, z + 1);
            }
        }

        return tris;
    }

    [System.Serializable]
    public struct Octave{
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
