using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCircle : MonoBehaviour
{
    public int dimToWrapAround;
    public float radius;
    public int density;
    public HyperbolicObject hypObjPref;

    public SphericalCam cam;


    private void Start()
    {

        for (int i = 0; i < density; i++)
        {
            for (int j = 0; j < density; j++)
            {
                for (int k = 0; k < density; k++)
                {
                    float a1 = 2 * i * Mathf.PI / density; // [0,180]
                    float a2 = 2 * j * Mathf.PI / density; // [0,180]
                    float a3 = 2 * k * Mathf.PI / density; // last angle is in range[0, 360)
                    HyperbolicObject o = Instantiate(hypObjPref);
                    o.Init(new Vector3(a1,a2,a3), radius, cam);
                }
            }
        }
    }
}
