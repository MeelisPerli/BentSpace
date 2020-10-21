using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCam : MonoBehaviour
{
    public bool invertY;
    public bool camOnSphere;
    public float speed = 1;
    public float radius = 1;

    public Matrix4x4 m;

    private void Start()
    {
        m = new Matrix4x4(
            new Vector4(Mathf.Cos(0.4f), Mathf.Sin(0.4f), 0, 0),
            new Vector4(-Mathf.Sin(0.4f), Mathf.Cos(0.4f), 0, 0),
            new Vector4(0, 0, Mathf.Cos(0.2f), Mathf.Sin(0.2f)),
            new Vector4(0, 0, -Mathf.Sin(0.2f), Mathf.Cos(0.2f))
        ) * new Matrix4x4(
            new Vector4(Mathf.Cos(0.4f), 0, Mathf.Sin(0.4f), 0),
            new Vector4(0, Mathf.Cos(0.2f), 0, Mathf.Sin(0.2f)),
            new Vector4(-Mathf.Sin(0.4f), 0, Mathf.Cos(0.4f), 0),
            new Vector4(0, -Mathf.Sin(0.2f), 0, Mathf.Cos(0.2f))
            );

    }

    void Update()
    {
        


        // Translation
        UpdateInputTranslationDirection();
    }


    void UpdateInputTranslationDirection()
    {
        
        Vector3 direction = new Vector3();
        Vector3 mouseMovement = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction -= Vector3.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= Vector3.right;
        }

        // Rotation
        if (Input.GetMouseButton(1))
        {
            mouseMovement = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1), 0) * 0.1f;
        }


        direction *= speed * Time.deltaTime;


        // https://www.shadertoy.com/view/WlGSDV

        // look left and right
        float s = Mathf.Sin(mouseMovement.z); 
        float c = Mathf.Cos(mouseMovement.z);
        Matrix4x4 p_xy = new Matrix4x4(
            new Vector4(c, s, 0, 0),
            new Vector4(-s, c, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1));

        // look up and down
        s = Mathf.Sin(mouseMovement.x);
        c = Mathf.Cos(mouseMovement.x);
        Matrix4x4 p_xz = new Matrix4x4(
            new Vector4(c, 0, s, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(-s, 0, c, 0),
            new Vector4(0, 0, 0, 1));

        // move forward and backward
        s = Mathf.Sin(direction.x);
        c = Mathf.Cos(direction.x);
        Matrix4x4 p_xw = new Matrix4x4(
            new Vector4(c, 0, 0, s),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(-s, 0, 0, c));

        // look cw roll
        s = Mathf.Sin(mouseMovement.y);
        c = Mathf.Cos(mouseMovement.y);
        Matrix4x4 p_yz = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, c, s, 0),
            new Vector4(0, -s, c, 0),
            new Vector4(0, 0, 0, 1));

        // move right and left
        s = Mathf.Sin(direction.y);
        c = Mathf.Cos(direction.y);
        Matrix4x4 p_yw = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, c, 0, s),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, -s, 0, c));

        // move up and down
        s = Mathf.Sin(direction.z);
        c = Mathf.Cos(direction.z);
        Matrix4x4 p_zw = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, c, s),
            new Vector4(0, 0, -s, c));

        m = m * p_xy * p_xz * p_xw * p_yz * p_yw * p_zw;
    }
}
