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

    private CharacterController controller;
    private Vector3 acceleration;
    private Vector3 velocity;
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

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateRotation();
        CheckForJump();
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * rotationVelocityFactor;

        transform.Rotate(0f, rotation, 0f);
    }

    private void CheckForJump()
    {
        if (!controller.isGrounded) return;

        if (Input.GetButton("Jump"))
            jumpChargeTime += Time.deltaTime;
        else if (Input.GetButtonUp("Jump"))
        {
            startJump = true;
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
        }
        else
            acceleration.y = gravityAcceleration;
    }

    private void UpdateVelocity()
    {
        velocity += acceleration * Time.fixedDeltaTime;

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

        motion = transform.TransformVector(motion);

        controller.Move(motion);
    }
}
