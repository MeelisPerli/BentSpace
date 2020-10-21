using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperbolicObject : MonoBehaviour
{
    public Vector3 sphericalCoords;
    public SphericalCam cam;
    public float radius;

    private Vector3 sloc;

    public void Init(Vector3 sphericalCoords, SphericalCam cam)
    {
        this.cam = cam;
        this.sphericalCoords = sphericalCoords;

    }

    // Update is called once per frame
    void Update()
    {
        //Transform();
    }

    /*
    private void Transform()
    {
        sloc = sphericalCoords + cam.sphericalCoords;
        Vector4 v4 = (new Vector4(
            Mathf.Cos(sloc.x),
            Mathf.Sin(sloc.x) * Mathf.Cos(sloc.y),
            Mathf.Sin(sloc.x) * Mathf.Sin(sloc.y) * Mathf.Cos(sloc.z),
            Mathf.Sin(sloc.x) * Mathf.Sin(sloc.y) * Mathf.Sin(sloc.z)
            ));

        transform.position = StereographicProjection(v4);
    }

    private Vector3 StereographicProjection(Vector4 v4)
    {
        float d = 1 - v4.w;
        return new Vector3(v4.x / d, v4.y / d, v4.z / d);
    }*/
}
