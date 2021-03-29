using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;
    [SerializeField] private float cameraRotationLimit = 85f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // gets a rotation vector 
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // gets a movement vector
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    // Get a force vector for thrusterforce
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }


    // Runs every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            // cam.transform.Rotate(-cameraRotationX);

            // Set camera rotation and clapm it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            // apply rotation to the transform of camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    // Perform movement based on velocity variable
}
