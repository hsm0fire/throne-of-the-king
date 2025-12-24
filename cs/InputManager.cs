using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControl PlayerControl;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    private float moveAmount;
    public float VerticalInput;
    public float HorizontalInput;

    PlayerCombatManager playerCombatManager;

    public bool jump_input;
    public bool attack_input;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }


    private void OnEnable()
    {
        if (PlayerControl == null)
        {
            PlayerControl = new PlayerControl();

            PlayerControl.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            PlayerControl.Player.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            PlayerControl.PlayerFunctions.Jump.performed += i => jump_input = true;
            PlayerControl.PlayerFunctions.Attack.performed += i => attack_input = true;
        }

        PlayerControl.Enable();
    }

    private void OnDisable()
    {
        PlayerControl.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
        HandleAttackInput();

    }

    private void HandleMovementInput()
    {
        VerticalInput = movementInput.y;
        HorizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }

    private void HandleJumpingInput()
    {
        if (jump_input)
        {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAttackInput()
    {
        if (attack_input)
        {
            animatorManager.PlayTargetAnimation("Attack_01", true, 1);
            attack_input = false;
        }
    }

}
