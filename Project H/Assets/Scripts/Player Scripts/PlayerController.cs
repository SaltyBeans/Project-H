using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    private float t;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    private PlayerMotor motor;
    private Vector3 _velocity;
    public bool isRunning { get { return running; } private set { } }
    private bool running;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        _velocity = Vector3.zero;
    }

    void Update()
    {

        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        _velocity = (_movHorizontal + _movVertical);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;


        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //Apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        speed = 5f;

        running = false;
        if (Input.GetKey(KeyCode.LeftShift)) //Sprint
        {
            speed = 10f;
            running = true;
        }

        //Apply rotation
        motor.Rotate(_rotation);

        //Apply movement
        motor.Move(_velocity.normalized, speed);
    }

    public bool IsGrounded()
    {
        return (Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f));
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}
