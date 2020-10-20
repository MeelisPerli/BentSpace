using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCam : MonoBehaviour
{
    public bool invertY;
    public bool camOnSphere;
    public float speed = 1;
    public float radius = 1;

    public Vector3 sphericalCoords;
    public Vector3 camRot;
    public Matrix4x4 m;

    private void Start()
    {
        sphericalCoords = new Vector3(0, 0, 0);
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
        // Rotation
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1)) * 0.1f;
            camRot = new Vector3(camRot.x + mouseMovement.y, camRot.y + mouseMovement.x, 0);
            //transform.rotation = Quaternion.Euler(r);
        }


        // Translation
        UpdateInputTranslationDirection();
    }


    void UpdateInputTranslationDirection()
    {
        
        Vector3 direction = new Vector3();

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

        if (Input.GetKeyDown(KeyCode.C)) {
            camOnSphere = camOnSphere == false;
            transform.position = new Vector3(0, 0, 0);
        }

        direction *= speed * Time.deltaTime;

        sphericalCoords += direction;

        if (sphericalCoords.x > 2 * Mathf.PI)
            sphericalCoords.x -= 2 * Mathf.PI;
        else if (sphericalCoords.x < 0)
            sphericalCoords.x += 2 * Mathf.PI;

        if (sphericalCoords.y > 2 * Mathf.PI)
            sphericalCoords.y -= 2 * Mathf.PI;
        else if (sphericalCoords.y < 0)
            sphericalCoords.y += 2 * Mathf.PI;

        if (sphericalCoords.z > 2 * Mathf.PI)
            sphericalCoords.z -= 2 * Mathf.PI;
        else if (sphericalCoords.z < 0)
            sphericalCoords.z += 2 * Mathf.PI;

        if (camOnSphere)
        {
            Vector4 v4 = radius * (new Vector4(
                            Mathf.Cos(sphericalCoords.x),
                            Mathf.Sin(sphericalCoords.x) * Mathf.Cos(sphericalCoords.y),
                            Mathf.Sin(sphericalCoords.x) * Mathf.Sin(sphericalCoords.y) * Mathf.Cos(sphericalCoords.z),
                            Mathf.Sin(sphericalCoords.x) * Mathf.Sin(sphericalCoords.y) * Mathf.Sin(sphericalCoords.z)
                            ));

            float d = radius - v4.w;
            transform.position = new Vector3(v4.x / d, v4.y / d, v4.z / d);
        }
        
    }
}
