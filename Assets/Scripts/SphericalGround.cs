using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGround : MonoBehaviour
{

    public SphericalCube hypCubePref;

    public Material smat1;
    public Material smat2;

    private SpaceManager sm;


    void Start()
    {
        sm = SpaceManager.Instance;
        GenerateGround();
    }


    private void GenerateGround()
    {
        for (int x = 0; x < sm.density; x++)
        {
            for (int y = 0; y < sm.density; y++)
            {
                Material material = smat1;
                if ((x + y)%2 == 0)
                {
                    material = smat2;
                }
                initcube(material, x, y, 0, 1);
            }
        }
    }


    private SphericalCube initcube(Material mat, int a, int b, int c, int d)
    {
        SphericalCube o = Instantiate(hypCubePref);
        o.material = mat;
        o.InitCube(sm.getGridPos(a), sm.getGridPos(b), sm.getGridPos(c), sm.getGridPos(d));
        return o;
    }
}
