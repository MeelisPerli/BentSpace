using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCam : MonoBehaviour
{
    public bool invertY;
    public bool flight;
    public float speed = 1;
    public float radius = 1;

    public Matrix4x4 m;
    public Vector3 posOn3Sphere;
    public float rotOn3Sphere;

    private Vector3 direction;
    private Vector3 mouseMovement;

    private void Start()
    {
        direction = new Vector3();
        mouseMovement = new Vector3();
        posOn3Sphere = new Vector3(0, 1.5f*Mathf.PI, 0);
        rotOn3Sphere = 0;

        m = SpaceManager.Instance.getYWRotMat(posOn3Sphere.y);
        /*
        m = new Matrix4x4(
            new Vector4(0.99612f, 0.07062f, 0.05237f, 0.00100f),
            new Vector4(-0.04707f, -0.00912f, 0.89924f, 0.43483f),
            new Vector4(-0.01473f, -0.09971f, 0.43167f, -0.89639f),
            new Vector4(0.07279f, -0.99246f, -0.04790f, 0.08613f)
        );
        */

        /*m = new Matrix4x4(
            new Vector4(Mathf.Cos(0.4f), Mathf.Sin(0.4f), 0, 0),
            new Vector4(-Mathf.Sin(0.4f), Mathf.Cos(0.4f), 0, 0),
            new Vector4(0, 0, Mathf.Cos(0.2f), Mathf.Sin(0.2f)),
            new Vector4(0, 0, -Mathf.Sin(0.2f), Mathf.Cos(0.2f))
        ) * new Matrix4x4(
            new Vector4(Mathf.Cos(0.4f), 0, Mathf.Sin(0.4f), 0),
            new Vector4(0, Mathf.Cos(0.2f), 0, Mathf.Sin(0.2f)),
            new Vector4(-Mathf.Sin(0.4f), 0, Mathf.Cos(0.4f), 0),
            new Vector4(0, -Mathf.Sin(0.2f), 0, Mathf.Cos(0.2f))
            );*/

    }

    void Update()
    {
        

        UpdateUserKeyboardInputs();
        UpdateUserMouseMovementInputs();

        if (flight)
        {
            UpdateInputTranslationDirection3DFlight();
        }
        else
        {
            UpdateInputTranslationDirection2DPlane();
        }
            


    }
    

    private void UpdateUserKeyboardInputs()
    {
        direction.x = 0;
        direction.y = 0;
        direction.z = 0;

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
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            flight = !flight;
            if (!flight)
            {
                m = SpaceManager.Instance.getYWRotMat(posOn3Sphere.y);
            }
            else
            {
                transform.rotation = new Quaternion();
            }
        }

        direction *= speed * Time.deltaTime;
    }

    private void UpdateUserMouseMovementInputs()
    {
        if (Input.GetMouseButton(1))
        {
            mouseMovement = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1), 0) * 0.1f;
            if (!flight)
            {
                float a = transform.rotation.eulerAngles.x + mouseMovement.y * 10000 * Time.deltaTime;
                if (a >= 180 && a < 275)
                {
                    a = -85;
                }
                else if (a > 85 && a < 180)
                {
                    a = 85;
                }

                transform.rotation = Quaternion.Euler(new Vector3(a, 0, 0));
                
                rotOn3Sphere +=  mouseMovement.x * 100 * Time.deltaTime;
                
            }   
        }
    }

    // Uses all 6 rotation matrices
    private void UpdateInputTranslationDirection3DFlight()
    {
        // https://www.shadertoy.com/view/WlGSDV

        // look cw roll
        Matrix4x4 p_xy = SpaceManager.Instance.getXYRotMat(mouseMovement.z);
        // look left and right
        Matrix4x4 p_xz = SpaceManager.Instance.getXZRotMat(mouseMovement.x);
        // look up and down
        Matrix4x4 p_yz = SpaceManager.Instance.getYZRotMat(mouseMovement.y);

        // move right and left
        Matrix4x4 p_xw = SpaceManager.Instance.getXWRotMat(direction.x);
        // move up and down
        Matrix4x4 p_yw = SpaceManager.Instance.getYWRotMat(direction.y);
        // move forward and backward
        Matrix4x4 p_zw = SpaceManager.Instance.getZWRotMat(direction.z);


        m = m * p_xy * p_xz * p_xw * p_yz * p_yw * p_zw;
    }

    // Deals with rotations in spherical coordinates, to make it able to walk on the surface
    private void UpdateInputTranslationDirection2DPlane()
    {

        Matrix4x4 look_r_l = SpaceManager.Instance.getXZRotMat(mouseMovement.x);

        Matrix4x4 move_r_l = SpaceManager.Instance.getXWRotMat(direction.x);
        Matrix4x4 move_f_b = SpaceManager.Instance.getZWRotMat(direction.z);

        m = m * look_r_l * move_r_l * move_f_b;

    }
}
