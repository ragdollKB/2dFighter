using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamBallGame.Visual
{
    public class IdleIndexSetter : StateMachineBehaviour
    {
        int idleIndex;

        void OnEnable()
        {
            idleIndex = Animator.StringToHash("IdleIndex");
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(idleIndex, Random.Range(0, 3));
        }
    }
}