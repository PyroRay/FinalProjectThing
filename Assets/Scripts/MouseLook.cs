using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Camera playerCamera;
    public float sprintFOV = 90.0f;
    public float runFOV = 60.0f;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.y, transform.localRotation.z);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKey("left shift") && Input.GetKey("w"))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, 10f * Time.deltaTime);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, runFOV, 10f * Time.deltaTime);
        }
    }
}
