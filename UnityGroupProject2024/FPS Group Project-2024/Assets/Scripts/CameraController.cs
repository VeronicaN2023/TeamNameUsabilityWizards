using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int LockMin, LockMax;
    [SerializeField] bool invertY;

    float rotateX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        if (invertY)
        {
            rotateX += mouseY;
        }
        else
        {
            rotateX -= mouseY;
        }

        rotateX = Mathf.Clamp(rotateX, LockMin, LockMax);

        transform.localRotation = Quaternion.Euler(rotateX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
