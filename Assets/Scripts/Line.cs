using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// taken and modified from https://mathcs.clarku.edu/~djoyce/poincare/PoincareB.html
public class Line
{

    public Vector2 A, B, C, D, P;
    public bool isStraight;
    public float r;


    public Line(Vector2 A, Vector2 B)
    {
        this.A = A; this.B = B;
        // first determine if its a line or a circle
        float den = A.x * B.y - B.x * A.y;
        isStraight = (Mathf.Abs(den) < 1.0e-14);
        if (isStraight)
        {
            P = A; // a point on the line}
                   // find a unit vector D in the direction of the line}
            den = Mathf.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y));
            D = new Vector2((B.x - A.x) / den,
                           (B.y - A.y) / den);
        }
        else
        { // it's a circle
          // find the center of the circle thru these points}
            float s1 = (1f + A.x * A.x + A.y * A.y) / 2.0f;
            float s2 = (1f + B.x * B.x + B.y * B.y) / 2.0f;
            C = new Vector2((s1 * B.y - s2 * A.y) / den,
                           (A.x * s2 - B.x * s1) / den);
            r = Mathf.Sqrt(C.x * C.x + C.y * C.y - 1.0f);
        } // if/else
    } // Line

    public Vector2 reflect(Vector2 R)
    {
        Vector2 Q = new Vector2();
        if (isStraight)
        {
            float factor = 2.0f * ((R.x - P.x) * D.x + (R.y - P.y) * D.y);
            Q.x = 2.0f * P.x + factor * D.x - R.x;
            Q.y = 2.0f * P.y + factor * D.y - R.y;
        }
        else
        {  // it's a circle
            float factor = r * r / ((R.x - C.x) * (R.x - C.x) + (R.y - C.y) * (R.y - C.y));
            Q.x = C.x + factor * (R.x - C.x);
            Q.y = C.y + factor * (R.y - C.y);
        } // if/else
        return Q;
    } // reflect
}
