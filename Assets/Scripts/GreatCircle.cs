using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCircle : MonoBehaviour
{
    public int dimToWrapAround;
    public int density;
    public HyperbolicObject hypObjPref;
    public HyperCube hypCubePref;
    public SphericalCam cam;

    public Material smat1;
    public Material smat2;
    public Material smat3;

    private float[] vertexSlots;


    private void Start()
    {
        GenHyperGrid();
        genHyperCubes();
    }

    private void genCubes()
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
                    o.Init(new Vector3(a1, a2, a3), cam);
                }
            }
        }
    }

    private void GenHyperGrid()
    {
        vertexSlots = new float[density];

        for (int i = 0; i < density; i++)
        {
            vertexSlots[i] = 2 * i * Mathf.PI / density;
        }
    }

    public float getGridPos(int a)
    {
        while (a < 0)
        {
            a += density;
        }
        return vertexSlots[a % density];
    }

    public void genHyperCubes()
    {

        for (int i = 0; i < density; i++)
        {
            Material mat;
            if (i % 2 == 0) mat = smat1;
            else mat = smat2;

            // head
            initcube(mat, i, 0, 0, 1);
            initcube(mat, i, density / 4, 0, 1);
            initcube(mat, density/4, i, 0, 1);

            initcube(mat, density / 4, density / 4, i, 1);
            initcube(mat, i, density / 4, density / 4, 1);
            initcube(mat, density / 4, i, density / 4, 1);

            //initcube(mat, i, 0, 0, 1);
            //initcube(mat, i, -1, 0, 1);
            //initcube(mat, i, 16, 0, 1);
            //initcube(mat, i, 0, 8, 1);
            //initcube(mat, 7, i, 0, 1);
            //initcube(mat, 8, i, 0, 1);

            //initcube(mat, 0, 0, i, 1);

            //initcube(mat, i, 16, 16, 1);
            //initcube(mat, 16, i, 16, 1);
            //initcube(mat, 16, 16, i, 1);
        }


        //HyperCube o = initcube(smat3, 5, 5, 5, 1);
        //o.moving = true;
    }

    private HyperCube initcube(Material mat, int a, int b, int c, int d)
    {
        HyperCube o = Instantiate(hypCubePref);
        o.material = mat;
        o.InitCube(getGridPos(a), getGridPos(b), getGridPos(c), getGridPos(d));
        return o;
    }

}
