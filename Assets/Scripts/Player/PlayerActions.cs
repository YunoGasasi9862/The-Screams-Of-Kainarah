using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerActions : MonoBehaviour
{
    private const float MAX_SLIDING_TIME_ALLOW = 0.5f;
    private PlayerInput _playerInput;
    private Rocky2DActions _rocky2DActions;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private PlayerAnimationMethods _animationHandler;
    private Vector2 _keystrokeTrack;
    private Command<bool> _jumpCommand;
    private CommandAsync<bool> _slideCommand;
    private IReceiver<bool> _jumpReceiver;
    private IReceiverAsync<bool> _slideReceiver;
    private IReceiver<bool> _attackReceiver;
    private Command<bool> _attackCommand;
    private float _timeForMouseClickStart=0f;
    private float _timeForMouseClickEnd=0f;

    [SerializeField] float _characterSpeed = 10f;

    public LedgeGrabController LedgeGrabController { get => GetComponent<LedgeGrabController>(); }
    public SlidingController SlidingController { get => GetComponent<SlidingController>(); }
    public JumpingController JumpingController { get => GetComponent<JumpingController>(); }
    public AttackingController AttackingController { get => GetComponent<AttackingController>(); } //implement all the actions together

    private bool GetJumpPressed { get; set; }
    private bool GetSlidePressed { get; set; }
    private float CharacterVelocityY { get; set; }
    private float CharacterVelocityX { get; set; }
    private float CharacterSpeed { get; set; }
    private float SlidingTimeBegin { get; set; }
    private float SlidingTimeEnd { get; set; }
    private float OriginalSpeed { get; set; }
    private bool LeftMouseButtonPressed { get; set; }
    private float TimeForMouseClickStart { get => _timeForMouseClickStart; set => _timeForMouseClickStart = value; }
    private float TimeForMouseClickEnd { get => _timeForMouseClickEnd; set => _timeForMouseClickEnd = value; }

    //Force = -2m * sqrt (g * h)
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rocky2DActions = new Rocky2DActions();// initializes the script of Rockey2Dactions
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationHandler = GetComponent<PlayerAnimationMethods>();
        _jumpReceiver = GetComponent<JumpingController>();
        _slideReceiver = GetComponent<SlidingController>();
        _attackReceiver = GetComponent<AttackingController>();
        _attackCommand = new Command<bool>(_attackReceiver);
        _jumpCommand = new Command<bool>(_jumpReceiver);
        _slideCommand = new CommandAsync<bool>(_slideReceiver);
        _rb = GetComponent<Rigidbody2D>();
        OriginalSpeed = _characterSpeed;

        _rocky2DActions.PlayerMovement.Jump.started += Jump; //i can add the same function
        _rocky2DActions.PlayerMovement.Jump.canceled += Jump;
        _rocky2DActions.PlayerMovement.Slide.started += BeginSlideAction;
        _rocky2DActions.PlayerMovement.Slide.canceled += EndSlideAction;

        _rocky2DActions.PlayerAttack.Attack.started += HandlePlayerAttackStart;
        _rocky2DActions.PlayerAttack.Attack.canceled += HandlePlayerAttackCancel;
        _rocky2DActions.PlayerAttack.ThrowProjectile.started += ThrowDaggerInput;
        _rocky2DActions.PlayerAttack.ThrowProjectile.canceled += ThrowDaggerInput;


    }

    private void Start()
    {
        _rocky2DActions.PlayerMovement.Enable(); //enables that actionMap =>Movement
        _rocky2DActions.PlayerAttack.Attack.Enable(); //activates the Action Map
        _rocky2DActions.PlayerAttack.ThrowProjectile.Enable();

        //event subscription
        JumpingController.onPlayerJumpEvent.AddListener(VelocityYEventHandler);
        SlidingController.onSlideEvent.AddListener(CharacterSpeedHandler);
    }
  
    private void Update()
    {
        if (!GameObjectCreator.GetDialogueManager().IsOpen())
        {
            //movement
            _keystrokeTrack = PlayerMovement();

            //Flipping
            if (KeystrokeMagnitudeChecker(_keystrokeTrack))
                FlipCharacter(_keystrokeTrack, ref _spriteRenderer);

            //jumping
            if(GetJumpPressed)
                _jumpCommand.Execute();
            else
                _jumpCommand.Cancel();

            //ledge grab
            if (PlayerVariables.Instance.IS_GRABBING) //tackles the ledgeGrab
            {
                LedgeGrabController.PerformLedgeGrab();
                return;
            }

            //sliding
            if(GetSlidePressed)
                _slideCommand.Execute();
            else
                _slideCommand.Cancel();
        }

    }
    private Vector2 PlayerMovement()
    {
        Vector2 keystroke = _rocky2DActions.PlayerMovement.Movement.ReadValue<Vector2>(); //reads the value

        CharacterVelocityX =  keystroke.x;

        CharacterControllerMove(CharacterVelocityX * CharacterSpeed, CharacterVelocityY);

        _animationHandler.RunningWalkingAnimation(keystroke.x);  //for movement, plays the animation

        CharacterSpeed = OriginalSpeed; //reset speed

        return keystroke;
    }
    
    private void VelocityYEventHandler(float characterVelocityY)
    {
        CharacterVelocityY = characterVelocityY;
    }
    private void CharacterSpeedHandler(float characterSpeed)
    {
        CharacterSpeed = characterSpeed;
    }
    private void CharacterControllerMove(float CharacterPositionX, float CharacterPositionY)
    {
        _rb.velocity = new Vector2(CharacterPositionX, CharacterPositionY);
    }

    private bool KeystrokeMagnitudeChecker(Vector2 _keystrokeTrack)
    {
        return _keystrokeTrack.magnitude != 0;
    }

    private bool FlipCharacter(Vector2 keystroke, ref SpriteRenderer _sr)
    {
        return keystroke.x >= 0 ? _sr.flipX = false : _sr.flipX = true; //flips the character
    }

    private void Jump(InputAction.CallbackContext context)
    {
        GetJumpPressed = GetSlidePressed? false: context.ReadValueAsButton();
    }

    private void BeginSlideAction(InputAction.CallbackContext context)
    {
        GetSlidePressed = (GetJumpPressed == true || PlayerVariables.Instance.IS_ATTACKING == true) ? false : context.ReadValueAsButton();
        SlidingTimeBegin = (float)context.time;
    }
    private void EndSlideAction(InputAction.CallbackContext context)
    {
        GetSlidePressed = (GetJumpPressed == true || PlayerVariables.Instance.IS_ATTACKING == true) ? false : context.ReadValueAsButton();
        SlidingTimeEnd = (float)context.time;
    }

    //implement boost feature with slide
    //To-Do


    //attacking mechanism centralized
    private void ThrowDaggerInput(InputAction.CallbackContext context)
    {

    }

    private void HandlePlayerAttackCancel(InputAction.CallbackContext context)
    {
        LeftMouseButtonPressed = (PlayerVariables.Instance.IS_SLIDING == true) ? false : context.ReadValueAsButton();
        TimeForMouseClickStart = (float)context.time;
    }

    private void HandlePlayerAttackStart(InputAction.CallbackContext context)
    {
        LeftMouseButtonPressed = (PlayerVariables.Instance.IS_SLIDING == true) ? false : context.ReadValueAsButton();
        TimeForMouseClickStart = (float)context.time;
    }

}