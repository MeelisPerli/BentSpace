using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCube : MonoBehaviour
{

    public Material material;

    private Vector3[] sVertices; // in spherical coords
    private Vector3[] vertices;

    public bool moving;

    private void Start()
    {
        //transform.parent = SpaceManager.Instance.cam.transform;
        //transform.position = new Vector3(0, 0, 0);
    }

    void Update() {
        if (moving)
        {
            sVertices = SpaceManager.Instance.MoveObjectAroundPoint(sVertices);
        }
    }

    public void InitCube(float x, float y, float z, float sideLen)
    {
        sVertices = new Vector3[8];
        vertices = new Vector3[8];
        int[] triangles = new int[36];

        // vertices

        sVertices[0] = new Vector3(x, y, z);
        sVertices[1] = new Vector3(x, y, z + sideLen);
        sVertices[2] = new Vector3(x, y + sideLen, z);
        sVertices[3] = new Vector3(x, y + sideLen, z + sideLen);

        sVertices[4] = new Vector3(x + sideLen, y, z);
        sVertices[5] = new Vector3(x + sideLen, y, z + sideLen);
        sVertices[6] = new Vector3(x + sideLen, y + sideLen, z);
        sVertices[7] = new Vector3(x + sideLen, y + sideLen, z + sideLen);

        // triangles

        //bottom
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 2;
        triangles[4] = 0;
        triangles[5] = 3;

        //top
        triangles[6] = 5;
        triangles[7] = 4;
        triangles[8] = 7;
        triangles[9] = 6;
        triangles[10] = 7;
        triangles[11] = 4;

        //side 1
        triangles[12] = 1;
        triangles[13] = 0;
        triangles[14] = 4;
        triangles[15] = 4;
        triangles[16] = 5;
        triangles[17] = 1;

        //side 2
        triangles[18] = 2;
        triangles[19] = 3;
        triangles[20] = 6;
        triangles[21] = 7;
        triangles[22] = 6;
        triangles[23] = 3;

        //side 3
        triangles[24] = 3;
        triangles[25] = 1;
        triangles[26] = 5;
        triangles[27] = 5;
        triangles[28] = 7;
        triangles[29] = 3;

        //side 4
        triangles[30] = 6;
        triangles[31] = 0;
        triangles[32] = 2;
        triangles[33] = 6;
        triangles[34] = 4;
        triangles[35] = 0;

        // transformations

        // init
        Mesh mesh = new Mesh();
        mesh.vertices = sVertices;
        mesh.triangles = triangles;

        // for disabling frustrum culling
        mesh.bounds = new Bounds(new Vector3(0,0,0), new Vector3(1000, 1000, 1000));

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
    }
}
