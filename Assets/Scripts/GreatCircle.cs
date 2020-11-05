using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCircle : MonoBehaviour
{
    public int dimToWrapAround;
    
    public SphericalCube hypCubePref;
    

    public Material smat1;
    public Material smat2;
    public Material smat3;

    private SpaceManager sm;
    


    private void Start()
    {
        sm = SpaceManager.Instance;
        genHyperCubes();
    }


    

    public void genHyperCubes()
    {

        for (int i = 0; i < sm.density; i++)
        {
            Material mat;
            if (i % 2 == 0) mat = smat1;
            else mat = smat2;

            initcube(mat, i, 0, 0, 1);
            initcube(mat, i, sm.density / 4, 0, 1);
            initcube(mat, sm.density /4, i, 0, 1);

            initcube(mat, sm.density / 4, sm.density / 4, i, 1);
            initcube(mat, i, sm.density / 4, sm.density / 4, 1);
            initcube(mat, sm.density / 4, i, sm.density / 4, 1);
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
