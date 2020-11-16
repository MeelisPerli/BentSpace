using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceInverter : MonoBehaviour
{

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
