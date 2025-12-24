using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion PlayerLocomotion;
    PlayerCombatManager playerCombatManager;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        PlayerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        PlayerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        PlayerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", PlayerLocomotion.isGrounded);

        inputManager.attack_input = false;
    }
}
