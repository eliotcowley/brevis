using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private bool canMove = true;
    [HideInInspector]
    public bool CanMove
    {
        get { return this.canMove; }
        set
        {
            if (!GameManager.Instance.GameOver)
            {
                this.canMove = value;
            }
        }
    }

    public float MoveSpeed = 2.5f;

    [SerializeField]
    private float minMoveSpeed = 0.5f;

    public float TurnSpeed = 10f;

    [SerializeField]
    private float moveSmooth = 1f;

    [SerializeField]
    private float moveThreshold = 0.05f;

    [SerializeField]
    private float stopSmooth = 1f;

    public Character PlayerCharacter;

    [HideInInspector]
    public string Primary;

    [HideInInspector]
    public string Secondary;

    [HideInInspector]
    public string Special;

    [HideInInspector]
    public Player Player
    {
        get
        {
            return this.PlayerCharacter == PersistentData.P1Character ? Player.Player1 : Player.Player2;
        }
    }

    [SerializeField]
    private float carrotWeightMultiplier = 0f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;
    private string horizontalInput;
    private string verticalInput;
    private bool controlsSwapped = false;
    private float horizontal;
    private float vertical;
    private Vector3 moveVector;
    private Vector3 targetVelocity;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Dog dog;
    private Rabbit rabbit;
    private float realMoveSpeed;

    public void PauseMovement()
    {
        this.CanMove = false;
        this.velocity = Vector3.zero;
        this.animator.SetFloat(Constants.Anim_Speed, 0f);
    }

    public void PauseMovement(float pauseTime)
    {
        StartCoroutine(PauseMovementCoroutine(pauseTime));
    }

    private IEnumerator PauseMovementCoroutine(float pauseTime)
    {
        PauseMovement();

        yield return new WaitForSeconds(pauseTime);

        this.CanMove = true;
    }

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        this.horizontalInput = (this.PlayerCharacter == PersistentData.P1Character) ? Constants.Input_P1Horizontal : Constants.Input_P2Horizontal;
        this.verticalInput = (this.PlayerCharacter == PersistentData.P1Character) ? Constants.Input_P1Vertical : Constants.Input_P2Vertical;

        this.controlsSwapped =
            ((this.PlayerCharacter == Character.Rabbit) && (this.PlayerCharacter != PersistentData.P1Character))
            || ((this.PlayerCharacter == Character.Dog) && (this.PlayerCharacter == PersistentData.P1Character));

        if (this.PlayerCharacter == Character.Dog)
        {
            this.dog = GetComponent<Dog>();
            this.Primary = InputManager.DogPrimary;
            this.Secondary = InputManager.DogSecondary;
            this.Special = InputManager.DogSpecial;
        }
        else
        {
            this.rabbit = GetComponent<Rabbit>();
            this.Primary = InputManager.RabbitPrimary;
            this.Secondary = InputManager.RabbitSecondary;
            this.Special = InputManager.RabbitSpecial;
        }
    }

    private void FixedUpdate()
    {
        //if (Input.GetButtonDown(InputManager.DogSpecial) && (this.dog != null) && this.dog.CanCharge && !this.dog.Stunned)
        //{
        //    this.CanMove = false;
        //    StartCoroutine(this.dog.Charge());
        //}

        if (!this.CanMove || (this.PlayerCharacter == Character.Dog && this.dog.Stunned))
        {
            return;
        }

        this.realMoveSpeed = GetMoveSpeed();
        GetMovementVectors();
        SetRotationVector();
        SetRotationPositionAndAnimator();
    }

    private void SetRotationPositionAndAnimator()
    {
        if (this.moveVector.sqrMagnitude > this.moveThreshold)
        {
            if (this.PlayerCharacter == Character.Rabbit)
            {
                this.rabbit.ResetHoleTimer();
            }

            this.rb.rotation = Quaternion.Lerp(this.rb.rotation, this.targetRotation, this.TurnSpeed * Time.fixedDeltaTime);
            Vector3.SmoothDamp(this.rb.position, this.targetPosition, ref this.velocity, this.moveSmooth, Mathf.Infinity, Time.fixedDeltaTime);
        }
        else
        {
            Vector3.SmoothDamp(this.rb.velocity, Vector3.zero, ref this.velocity, this.stopSmooth, Mathf.Infinity, Time.fixedDeltaTime);
        }

        this.rb.velocity = new Vector3(this.velocity.x, this.rb.velocity.y, this.velocity.z);
        this.animator.SetFloat(Constants.Anim_Speed, this.rb.velocity.sqrMagnitude);
    }

    private void SetRotationVector()
    {
        if (this.moveVector != Vector3.zero)
        {
            this.targetRotation = Quaternion.LookRotation(this.moveVector, Vector3.up);
        }
    }

    private float GetMoveSpeed()
    {
        float moveSpeed = this.MoveSpeed;

        if (this.PlayerCharacter == Character.Rabbit)
        {
            float moveSpeedWithCarrots = this.MoveSpeed - (GameManager.Instance.Carrots * this.carrotWeightMultiplier);
            moveSpeed = (moveSpeedWithCarrots < this.minMoveSpeed) ? this.minMoveSpeed : moveSpeedWithCarrots;
        }

        return moveSpeed;
    }

    private void GetMovementVectors()
    {
        this.horizontal = Input.GetAxis(this.horizontalInput);
        this.vertical = Input.GetAxis(this.verticalInput);

        if (this.controlsSwapped)
        {
            this.horizontal *= -1f;
            this.vertical *= -1f;
        }

        this.moveVector = new Vector3(this.horizontal, 0f, this.vertical);

        if (this.moveVector.magnitude > 1f)
        {
            this.moveVector.Normalize();
        }

        this.targetVelocity = this.moveVector * this.realMoveSpeed;
        this.targetPosition = this.rb.position + this.targetVelocity;
        this.targetRotation = Quaternion.identity;
    }
}
