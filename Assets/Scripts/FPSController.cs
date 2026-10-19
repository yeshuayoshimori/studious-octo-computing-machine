using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpforce = 5f;

    [Header("Mouse Look")]
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float maxLookAngle = 80f;

    private Rigidbody _rb;
    private Camera _camera;
    private float _verticalRotation = 0f;
    private bool _isGrounded = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.mass = 1f;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        _camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0, mouseX, 0); //x, y z

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -maxLookAngle, maxLookAngle); //restricting direction
        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate() //multiple of hundreed
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.forward * vertical + transform.right * horizontal).normalized;
        Vector3 targetVelocity = move * moveSpeed;

        _rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);


        //targetVelocity.y = _rb.linearVelocity.y;
        //_rb.linearVelocity = targetVelocity;
    }

    private void OnCollisionStay(Collision collision) //brand new instance (BLUE ONE) Each time a collition is contacting in various places as well.//Contact points
    {
        foreach (ContactPoint contact in collision.contacts) //if in ground why shuld we check the 4 if if we have 1 
        {
            if (contact.normal.y > 0.5) //means is in the ground
            {
                _isGrounded = true;
                return;
            }
        }
    }


    private void OnCollisionExit(Collision collision) //if i leave the collision
    {
        _isGrounded = false;
    }

}

    //public -> scripts can access it / exposed to unity by default
    //private -> scripts can't access it / not exposed to unity by default
    // [HideInInspector] public -> scripts can access it / not exposed
    // [SerializeField] -> scripts can't access it / exposed in unity

    
