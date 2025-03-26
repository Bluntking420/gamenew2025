using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(0, 2, -5); // Offset from the player
    public float sensitivity = 5f; // Mouse sensitivity
    public float rotationSmoothTime = 0.12f;

    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // Limit vertical rotation

        // Smooth rotation
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        // Apply rotation and position
        transform.position = player.position + Quaternion.Euler(currentRotation) * offset;
        transform.LookAt(player.position);
    }
}
