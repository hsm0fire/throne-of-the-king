using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    PlayerManager playerManager;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleLightAttack()
    {
        // 1. الوصول إلى الـ Rigidbody وتصفير السرعة فوراً لمنع الانزلاق
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // هذا السطر يوقف الانزلاق الناتج عن W,A,S,D
        }

        // 2. تشغيل الأنيميشن وتفعيل التفاعل (isInteracting)
        animatorManager.PlayTargetAnimation("Attack_01", true, 1);
    }
}