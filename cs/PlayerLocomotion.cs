using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager PlayerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    [Header("Falling")]
    public float inAirTimer;
    public float fallingVelocity;
    public float leapingVelocity;
    public float rayCastOffSet = 0.5f;
    public LayerMask groundLayer;

    public float movementSpeed = 6;
    public float rotationSpeed = 15;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping;

    [Header("Jumping")]
    public float jumingHight = 3;
    public float gravityIntensity = -15;

    private void Awake()
    {
        PlayerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {

        HandleFallingAndLanding();

        if (PlayerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        if (isJumping)
            return;
        moveDirection = cameraObject.forward * inputManager.VerticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.VerticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.HorizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!PlayerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true, 1);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !PlayerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Landing", true, 1);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false, 1);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumingHight);
            Vector3 plyerVelocity = moveDirection;
            plyerVelocity.y = jumpingVelocity;
            rb.velocity = plyerVelocity;
        }
    }
}
