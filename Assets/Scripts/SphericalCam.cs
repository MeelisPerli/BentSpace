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

    private void Start()
    {
        sphericalCoords = new Vector3(5, 0, 0);
    }

    void Update()
    {
        // Rotation
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + mouseMovement.y, transform.rotation.eulerAngles.y + mouseMovement.x, 0);
        }


        // Translation
        UpdateInputTranslationDirection();
    }

    void UpdateInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction.z += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z -= speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.y += speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.y -= speed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction.x -= speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction.x += speed;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            camOnSphere = camOnSphere == false;
        }

        direction *= Time.deltaTime;

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
        } else
        {
            transform.position = new Vector3();
        }

    }
}
