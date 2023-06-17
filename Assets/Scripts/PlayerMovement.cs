using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float backwardAcceleration;
    [SerializeField] private float strafeAcceleration;
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private float horizontalJumpAcceleration;
    [SerializeField] private float maxForwardVelocity;
    [SerializeField] private float maxBackwardVelocity;
    [SerializeField] private float maxStrafeVelocity;
    [SerializeField] private float maxFallVelocity;
    [SerializeField] private float rotationVelocityFactor;
    [SerializeField] private ResourceBar jumpBar;
    [SerializeField] private float maxJumpCharge;
    [SerializeField] private GameObject model;

    [SerializeField] private float waterVelocityFactor;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Transform waterCheckTransform;
    [SerializeField] private float waterCheckRadius;

    private CharacterController controller;
    private Vector3 acceleration;
    private Vector3 velocity;
    private Quaternion movementDirection;
    private bool startJump;
    private float sinPI4;
    private float jumpChargeTime;

    public float JumpChargePower => jumpChargeTime;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        startJump = false;
        sinPI4 = Mathf.Sin(Mathf.PI / 4);
        jumpChargeTime = 0f;

        jumpBar.SetFill(0);

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckForJump();

        // Update movement direction only if player is grounded
        if (controller.isGrounded)
        {
            Quaternion cameraOrientation = Camera.main.transform.rotation;
            movementDirection = Quaternion.AngleAxis(cameraOrientation.eulerAngles.y, Vector3.up);
            RotateModel();
        }
    }

    private void RotateModel()
    {
        Quaternion rotation = model.transform.rotation;

        Vector3 movementInput = new Vector3(Input.GetAxis("Strafe"), 0f,
            Input.GetAxis("Forward"));

        if (startJump)
        {
            rotation = movementDirection;
        }
        else if (movementInput.magnitude >= 0.01f)
        {
            // How much the model needs to rotate relative to the camera
            float angle = Mathf.Atan2(movementInput.x, movementInput.z)
                * Mathf.Rad2Deg;
            rotation = Quaternion.AngleAxis(angle, Vector3.up) * movementDirection;
        }

        model.transform.rotation = rotation;
    }

    private void CheckForJump()
    {
        if (!controller.isGrounded) return;

        if (Input.GetButton("Jump"))
        {
            jumpChargeTime = Mathf.Min(maxJumpCharge, jumpChargeTime + Time.deltaTime);

            jumpBar.SetFill(jumpChargeTime / maxJumpCharge);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            startJump = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Jump", transform.position);
        }
    }

    void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        UpdateForwardAcceleration();
        UpdateStrafeAcceleration();
        UpdateVerticalAcceleration();
    }

    private void UpdateForwardAcceleration()
    {
        if (startJump)
        {
            acceleration.z = horizontalJumpAcceleration;
        }
        else if (controller.isGrounded)
        {
            float forwardAxis = Input.GetAxis("Forward");

            if (forwardAxis > 0f)
                acceleration.z = forwardAcceleration;
            else if (forwardAxis < 0f)
                acceleration.z = backwardAcceleration;
            else
                acceleration.z = 0f;
        }
    }

    private void UpdateStrafeAcceleration()
    {
        if (startJump)
        {
            acceleration.x = 0f;
        }
        else if (controller.isGrounded)
        {
            float strafeAxis = Input.GetAxis("Strafe");

            if (strafeAxis > 0f)
                acceleration.x = strafeAcceleration;
            else if (strafeAxis < 0f)
                acceleration.x = -strafeAcceleration;
            else
                acceleration.x = 0f;
        }
    }

    private void UpdateVerticalAcceleration()
    {
        if (startJump)
        {
            acceleration.y = jumpAcceleration * Mathf.Clamp(jumpChargeTime, 0.5f, 1.5f);
            jumpChargeTime = 0f;

            jumpBar.SetFill(0);
        }
        else
            acceleration.y = gravityAcceleration;
    }

    private void UpdateVelocity()
    {
        velocity += acceleration * Time.fixedDeltaTime;

        // If we're in water, scale down velocity
        if (Physics.CheckSphere(waterCheckTransform.position, waterCheckRadius, waterLayer))
        {
            velocity = velocity * waterVelocityFactor;
        }

        if (acceleration.z == 0f || (acceleration.z * velocity.z < 0f))
            velocity.z = 0f;
        else if (acceleration.x == 0f)
            velocity.z = Mathf.Clamp(velocity.z, maxBackwardVelocity, maxForwardVelocity);
        else
            velocity.z = Mathf.Clamp(velocity.z, maxBackwardVelocity * sinPI4, maxForwardVelocity * sinPI4);

        if (acceleration.x == 0f || (acceleration.x * velocity.x < 0f))
            velocity.x = 0f;
        else if (acceleration.z == 0f)
            velocity.x = Mathf.Clamp(velocity.x, -maxStrafeVelocity, maxStrafeVelocity);
        else
            velocity.x = Mathf.Clamp(velocity.x, -maxStrafeVelocity * sinPI4, maxStrafeVelocity * sinPI4);

        if (controller.isGrounded && !startJump)
            velocity.y = -0.1f;
        else
            velocity.y = Mathf.Max(velocity.y, maxFallVelocity);

        startJump = false;
    }

    private void UpdatePosition()
    {
        Vector3 motion = velocity * Time.fixedDeltaTime;

        motion = movementDirection * motion;

        controller.Move(motion);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(waterCheckTransform.position, waterCheckRadius);
    }
}
