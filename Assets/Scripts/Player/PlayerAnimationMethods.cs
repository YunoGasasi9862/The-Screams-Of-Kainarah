using PlayerAnimationHandler;
using UnityEngine;

public class PlayerAnimationMethods : MonoBehaviour
{
    private AnimationStateMachine _stateMachine;
    private Animator _anim;
    private float _maxSlideTime = 0.4f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        if (_anim != null)
            _stateMachine = new AnimationStateMachine(_anim); // initializing the object
    }

    private void Update()
    {
        if (_anim != null && _anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationConstants.SLIDING) &&
            ReturnCurrentAnimation() > _maxSlideTime)
        {
            PlayAnimation(AnimationConstants.SLIDING, false);  //for fixing the Sliding Issue
        }

    }

    public bool VectorChecker(float compositionX)
    {
        return compositionX != 0f;
    }

    private void PlayAnimation(string name, int state)
    {
        _stateMachine.AnimationPlayForInt(name, state);
    }
    private void PlayAnimation(string name, bool state)
    {
        _stateMachine.AnimationPlayForBool(name, state);
    }
    private void PlayAnimation(string name, float state)
    {
        _stateMachine.AnimationPlayForFloat(name, state);
    }
    public void RunningWalkingAnimation(float keystroke)
    {
        if (VectorChecker(keystroke) && !PlayerVariables.Instance.IS_JUMPING)
        {
            AnimationStateKeeper.CurrentPlayerState = (int)AnimationStateKeeper.StateKeeper.RUNNING;
            SetMovementStates(true, false);
            PlayAnimation(AnimationConstants.MOVEMENT, AnimationStateKeeper.CurrentPlayerState);
        }

        if (!VectorChecker(keystroke) && !PlayerVariables.Instance.IS_JUMPING)
        {
            AnimationStateKeeper.CurrentPlayerState = (int)AnimationStateKeeper.StateKeeper.IDLE;
            SetMovementStates(false, true);
            PlayAnimation(AnimationConstants.MOVEMENT, AnimationStateKeeper.CurrentPlayerState);
        }

    }

    private void SetMovementStates(bool isRunning, bool isWalking)
    {
        PlayerVariables.Instance.runVariableEvent.Invoke(isRunning);
        PlayerVariables.Instance.walkVariableEvent.Invoke(isWalking);
    }

    public void JumpingFallingAnimationHandler(bool keystroke)
    {
        AnimationStateKeeper.CurrentPlayerState = keystroke
            ? (int)AnimationStateKeeper.StateKeeper.JUMP
            : (int)AnimationStateKeeper.StateKeeper.FALL;
        PlayAnimation(AnimationConstants.MOVEMENT, AnimationStateKeeper.CurrentPlayerState);
    }
    public void UpdateJumpTime(string parameterName, float jumpTime)
    {
        PlayAnimation(parameterName, jumpTime);
    }

    public void Sliding(bool keystroke)
    {
        PlayAnimation(AnimationConstants.SLIDING, keystroke);
    }

    public float ReturnCurrentAnimation()
    {
        return _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool IsNameOfTheCurrentAnimation(string name)
    {
        return _anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public Animator getAnimator()
    {
        return _anim;
    }


}