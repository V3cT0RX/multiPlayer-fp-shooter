using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float thrusterForce = 1000f;
    [Header("Spring Settings:")]
    // [SerializeField] private JointProjectionMode projectionMode = JointProjectionMode.PositionAndRotation;
    // [SerializeField] private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 40f;
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSetting(jointSpring);
    }
    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
        motor.Move(_velocity);

        // Calculate rotation as a 3d vector (turing around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        // Apply Rotation
        motor.Rotate(_rotation);


        // Calculate Camera rotation as a 3d vector (turing around)
        float _xRot = Input.GetAxisRaw("Mouse Y");
        // Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;
        float _cameraRotationX = _xRot * lookSensitivity;
        // Apply CameraRotation
        motor.RotateCamera(_cameraRotationX);

        // Calculate the thrusterforce on Input
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSetting(0f);
        }
        else
        {
            SetJointSetting(jointSpring);
        }
        // Apply the thruster Force
        motor.ApplyThruster(_thrusterForce);

    }
    private void SetJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
        // projectionMode = JointProjectionMode,
    }
}
