
using UnityEngine;
namespace PlayerAnimationHandler
{
    public class AnimationConstants
    {

        public const string MOVEMENT = "State";
        public const string SLIDING = "Sliding";
        public const string LEDGEGRAB = "LedgeGrab";
        public const string THROWDAGGER = "ThrowDagger";


    }

    public class AnimationStateKeeper
    {
        public static int currentPlayerState = 0;

        public enum StateKeeper
        {
            IDLE = 0, RUNNING = 1, JUMP = 2, FALL = 3, SLIDING = 4
        }


    }

    public class AnimationStateMachine
    {
        private Animator animator; //each object will have its own AnimationStateMachine

        public AnimationStateMachine(Animator animator)
        {
            this.animator = animator;
        }

        public void AnimationPlayMachineInt(string constName, int state)
        {
            animator.SetInteger(constName, state);

        }
        public void AnimationPlayMachineBool(string constName, bool state)
        {
            animator.SetBool(constName, state);
        }

    }


}