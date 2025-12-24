using UnityEngine;

public class ResetIsInteracting : StateMachineBehaviour
{
    // يتم استدعاء هذه الدالة فور الخروج من حالة الأنيميشن في الـ Animator
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInteracting", false);
    }
}