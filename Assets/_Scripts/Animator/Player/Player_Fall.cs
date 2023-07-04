using UnityEngine;

public class Player_Fall : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<PlayerFXManager>() == null)
            return;
        animator.GetComponent<PlayerFXManager>().Update_Fall(true);
    }
}
