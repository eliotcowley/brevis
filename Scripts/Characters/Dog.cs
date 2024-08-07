using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class Dog : MonoBehaviour
{
    [HideInInspector]
    public bool CanCharge = true;

    [HideInInspector]
    public bool Stunned = false;

    [HideInInspector]
    public bool PauseAfterRps = false;

    public float afterRpsCantAttackTime = 0.5f;

    [SerializeField]
    private float attackDuration = 0.9f;

    [SerializeField]
    private GameObject rabbitGameObject;

    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private float arrowXRotationOffset = 90f;

    [SerializeField]
    private float trackTime = 3f;

    [SerializeField]
    private float chargeCooldown = 5f;

    [SerializeField]
    private float chargeThrust = 1f;

    [SerializeField]
    private float trackingSpeed = 0.5f;

    [SerializeField]
    private AudioClip dogAttack;
    [SerializeField]
    private TextMeshProUGUI chargeTimerText;

    [SerializeField]
    private float pauseBeforeCharge = 0.2f;

    [SerializeField]
    private ParticleSystem chargeEffect;

    [SerializeField]
    private float stunSeconds = 7f;

    [SerializeField]
    private ParticleSystem stunEffect;

    private bool canHurtRabbit = true;
    private Animator animator;
    private Animator rabbitAnimator;
    private bool isAttacking = false;
    private string inputPrimary;
    private string inputSecondary;
    private bool nearRabbit = false;
    private bool isTracking = false;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private float moveSmoothOriginal;
    private float chargeTimer = 0f;
    private TrailRenderer scentTrail;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rabbitAnimator = this.rabbitGameObject.GetComponent<Animator>();
        this.inputPrimary = (PersistentData.P1Character == Character.Dog) ? Constants.Input_P1Primary : Constants.Input_P2Primary;
        this.inputSecondary = (PersistentData.P1Character == Character.Dog) ? Constants.Input_P1Secondary : Constants.Input_P2Secondary;
        this.arrow.gameObject.layer = (PersistentData.P1Character == Character.Dog) ? LayerMask.NameToLayer(Constants.Layer_P1Only) : LayerMask.NameToLayer(Constants.Layer_P2Only);
        this.arrow.GetChild(0).gameObject.layer = this.arrow.gameObject.layer;
        this.arrow.gameObject.SetActive(false);
        this.rb = GetComponent<Rigidbody>();
        this.playerMovement = GetComponent<PlayerMovement>();
        this.moveSmoothOriginal = this.playerMovement.MoveSpeed;
        this.scentTrail = this.rabbitGameObject.GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        // Since we're removing secondary and tertiary attacks, I'm commenting this out. Can't bring myself to delete it.
        if (!this.Stunned && !this.PauseAfterRps)
        {
            /*if (Input.GetButtonDown(this.inputPrimary) && !this.isAttacking)
            {
                StartCoroutine(Attack());
            }
            else */

            if (Input.GetButtonDown(this.inputSecondary))
            {
                Track();
            }

        //    if (this.canHurtRabbit && this.nearRabbit && this.isAttacking)
        //    {
        //        DropCarrotsOrWin();
        //    }
        }

        if (Input.GetButtonUp(this.inputSecondary))
        {
            StopTracking();
        }

        // Check for distance between players and toggle dog's arrow when tracking
        if (this.isTracking)
        {
            if (Vector3.Distance(this.transform.position, this.rabbitGameObject.transform.position) < Constants.Camera_Toggle_Distance && this.arrow.gameObject.activeSelf)
            {
                this.arrow.gameObject.SetActive(false);
            }
            else if (Vector3.Distance(this.transform.position, this.rabbitGameObject.transform.position) >= Constants.Camera_Toggle_Distance && !this.arrow.gameObject.activeSelf)
            {
                this.arrow.gameObject.SetActive(true);
            }
        }

        //Vector3 arrowVector = this.rabbitGameObject.transform.position - this.transform.position;
        //Quaternion towardsRabbit = Quaternion.LookRotation(arrowVector);
        //Vector3 newVec = new Vector3(towardsRabbit.eulerAngles.x + this.arrowXRotationOffset, towardsRabbit.eulerAngles.y, towardsRabbit.eulerAngles.z);
        //this.arrow.rotation = Quaternion.Euler(newVec);

        this.arrow.LookAt(this.rabbitGameObject.transform);

        //if (this.CanCharge == false)
        //{
        //    this.chargeTimer += Time.deltaTime;
        //    this.chargeTimerText.SetText(Mathf.Ceil(this.chargeCooldown - this.chargeTimer).ToString());

        //    if (this.chargeTimer >= this.chargeCooldown)
        //    {
        //        this.CanCharge = true;
        //    }
        //}
        //else
        //{
        //    this.chargeTimerText.SetText("");
        //}
    }

    public void DropCarrotsOrWin()
    {
        this.animator.SetTrigger(Constants.Anim_Attack);
        GameManager.Instance.PlaySound(this.dogAttack);

        if (GameManager.Instance.GetCarrots() > 0)
        {
            GameManager.Instance.DropAllCarrots();
            StartCoroutine(MakeRabbitInvincible(this.rabbitGameObject));
            this.rabbitAnimator.SetTrigger(Constants.Anim_Damaged);
        }
        else
        {
            this.rabbitAnimator.SetTrigger(Constants.Anim_Die);
            this.rabbitAnimator.SetInteger(Constants.Anim_Lives, GameManager.Instance.Lives);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_Rabbit))
        {
            this.nearRabbit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_Rabbit))
        {
            this.nearRabbit = false;
        }
    }

    private IEnumerator MakeRabbitInvincible(GameObject rabbit)
    {
        this.canHurtRabbit = false;
        GameManager.Instance.BlinkObject(rabbit, Constants.Rabbit_IFrames, 0.2f);
        yield return new WaitForSeconds(Constants.Rabbit_IFrames);
        this.canHurtRabbit = true;
    }

    private IEnumerator Attack()
    {
        this.isAttacking = true;
        this.animator.SetTrigger(Constants.Anim_Attack);
        GameManager.Instance.PlaySound(this.dogAttack);
        yield return new WaitForSeconds(this.attackDuration);
        this.isAttacking = false;
        this.playerMovement.CanMove = true;
    }

    public void Track()
    {
        if (!this.isTracking)
        {
            this.isTracking = true;
            this.animator.SetBool(Constants.Anim_Sniff, true);
            this.scentTrail.gameObject.layer = (PersistentData.P1Character == Character.Dog) ? LayerMask.NameToLayer(Constants.Layer_P1Only) : LayerMask.NameToLayer(Constants.Layer_P2Only);

            this.playerMovement.MoveSpeed = this.trackingSpeed;
        }
    }

    private void StopTracking()
    {
        this.arrow.gameObject.SetActive(false);
        this.scentTrail.gameObject.layer = LayerMask.NameToLayer(Constants.Layer_Invisible);
        this.animator.SetBool(Constants.Anim_Sniff, false);
        this.isTracking = false;
        this.playerMovement.MoveSpeed = this.moveSmoothOriginal;
    }

    public IEnumerator Charge()
    {
        this.CanCharge = false;
        this.chargeTimer = 0f;
        this.chargeEffect.Play();
        yield return new WaitForSeconds(this.pauseBeforeCharge);
        this.rb.velocity = this.transform.forward * this.chargeThrust;
        StartCoroutine(Attack());
    }

    public IEnumerator Stun()
    {
        this.Stunned = true;
        this.stunEffect.Play();
        this.animator.SetFloat(Constants.Anim_Speed, 0f);
        yield return new WaitForSeconds(this.stunSeconds);
        this.Stunned = false;
        this.stunEffect.Stop();
        this.stunEffect.Clear();
    }
}
