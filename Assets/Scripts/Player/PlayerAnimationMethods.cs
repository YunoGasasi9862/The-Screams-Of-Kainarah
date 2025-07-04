using PlayerAnimationHandler;
using System.Threading;
using UnityEngine;

public class PlayerAnimationMethods : MonoBehaviour, IObserver<PlayerSystem>
{
    private PlayerSystemDelegator PlayerSystemDelegator { get; set; }

    private AnimationStateMachine _stateMachine;

    private Animator _anim;

    private float _maxSlideTime = 0.4f;

    private PlayerSystem PlayerSystem { get; set; }

    private void Awake()
    {
        _stateMachine = new AnimationStateMachine(GetComponent<Animator>());

        PlayerSystemDelegator = Helper.GetDelegator<PlayerSystemDelegator>();
    }

    private void Start()
    {
        StartCoroutine(PlayerSystemDelegator.NotifySubject(this, new NotificationContext()
        {
            ObserverName = gameObject.name,
            ObserverTag = gameObject.tag,
            SubjectType = typeof(PlayerSystem).ToString()
        }, CancellationToken.None));

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
        if (PlayerSystem == null)
        {
            Debug.Log("PlayerSystem is null for [PlayerAnimationMethods - RunningWalkingAnimation] - exiting!");
            return;
        }

        if (VectorChecker(keystroke) && !PlayerSystem.IS_JUMPING)
        {
            UpdateMovementState(AnimationStateKeeper.StateKeeper.RUNNING, true, false);

        }

        if (!VectorChecker(keystroke) && !PlayerSystem.IS_JUMPING)
        {
            UpdateMovementState(AnimationStateKeeper.StateKeeper.IDLE, false, true);
        }

    }

    private void SetMovementStates(bool isRunning, bool isWalking)
    {
        //make it better, make sure delegators get their data first before any operations get executed
        if (PlayerSystem == null)
        {
            Debug.Log("PlayerSystem is null for [PlayerAnimationMethods - SetMovementStates] - exiting!");
            return;
        }

        PlayerSystem.runVariableEvent.Invoke(isRunning);

        PlayerSystem.walkVariableEvent.Invoke(isWalking);
    }

    public void UpdateMovementState(AnimationStateKeeper.StateKeeper state, bool isRunning, bool isWalking)
    {
        AnimationStateKeeper.CurrentPlayerState = (int)state;
        SetMovementStates(isRunning, isWalking);
        PlayAnimation(AnimationConstants.MOVEMENT, AnimationStateKeeper.CurrentPlayerState);
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

    public void OnNotify(PlayerSystem data, NotificationContext notificationContext, SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken, params object[] optional)
    {
        PlayerSystem = data;
    }
}