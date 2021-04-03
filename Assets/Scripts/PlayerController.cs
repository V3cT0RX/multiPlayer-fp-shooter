using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float thrusterForce = 1000f;
    [SerializeField] private float thrusterFuelBurnSpeed = 1f;
    [SerializeField] private float thrusterFuelRegenSpeed = 1f;
    private float thrusterFuelAmount = 1f;
    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField] private LayerMask environmentMask;

    [Header("Spring Settings:")]
    // [SerializeField] private JointProjectionMode projectionMode = JointProjectionMode.PositionAndRotation;
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 40f;

    // Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSetting(jointSpring);
    }
    void Update()
    {
        /*
        Setting target position for spring 
        This make the physics act right when it comes to 
        applying gravity when flying over objects
        */

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;
        // Apply movement
        animator.SetFloat("ForwardVelocity", _zMov);
        // Apply movement
        motor.Move(_velocity);

        // Calculate rotation as a 3d vector (turing around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        // Apply Rotation
        motor.Rotate(_rotation);


        // Calculate Camerarotation as a 3d vector (turing around)
        float _xRot = Input.GetAxisRaw("Mouse Y");
        // Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;
        float _cameraRotationX = _xRot * lookSensitivity;
        // Apply CameraRotation
        motor.RotateCamera(_cameraRotationX);


        // Calculate the thrusterforce on Input
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime; // fuel burns
            if (thrusterFuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSetting(0f);
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime; //fuel refill
            SetJointSetting(jointSpring);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);
        // Apply the thruster Force
        motor.ApplyThruster(_thrusterForce);


    }
    private void SetJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
        // projectionMode = JointProjectionMode,
    }
}
