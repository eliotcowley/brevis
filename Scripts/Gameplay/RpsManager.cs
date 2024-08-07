using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RpsManager : MonoBehaviour
{
    public static RpsManager Instance;

    [HideInInspector]
    public bool InBulletTime;

    [HideInInspector]
    public RockPaperScissors RabbitRps;

    [HideInInspector]
    public RockPaperScissors DogRps;

    public float BulletTimeScale = 0.1f;
    public float BulletTimeSeconds = 3f;

    [SerializeField]
    private float secondsToShowRpsChoices = 3f;

    [SerializeField]
    private float knockbackAmount = 20f;

    [SerializeField]
    private AudioClip countdownClip;
    
    private float bulletTimer = 0f;
    private float scaledSeconds;
    private Dog dog;
    private Animator dogAnimator;
    private Animator rabbitAnimator;
    private Rigidbody rabbitRb;
    private Rigidbody dogRb;
    private PlayerMovement rabbitMovement;
    private PlayerMovement dogMovement;
    private Coroutine showRpsCoroutine;
    private Coroutine bulletTimeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("RpsManager already exists");
        }
    }

    private void Start()
    {
        this.RabbitRps = GameManager.Instance.Rabbit.GetComponent<RockPaperScissors>();
        this.DogRps = GameManager.Instance.Dog.GetComponent<RockPaperScissors>();
        this.scaledSeconds = this.BulletTimeSeconds * this.BulletTimeScale;
        this.dog = GameManager.Instance.Dog.GetComponent<Dog>();
        this.dogAnimator = GameManager.Instance.Dog.GetComponent<Animator>();
        this.rabbitAnimator = GameManager.Instance.Rabbit.GetComponent<Animator>();
        this.rabbitRb = GameManager.Instance.Rabbit.GetComponent<Rigidbody>();
        this.dogRb = GameManager.Instance.Dog.GetComponent<Rigidbody>();
        this.rabbitMovement = GameManager.Instance.Rabbit.GetComponent<PlayerMovement>();
        this.dogMovement = GameManager.Instance.Dog.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (this.InBulletTime)
        {
            this.bulletTimer -= Time.deltaTime;
            UIManager.Instance.SetRpsTimer(this.bulletTimer / this.scaledSeconds);
        }
    }

    public void UnlockRpsChoices()
    {
        this.RabbitRps.RpsLocked = false;
        this.RabbitRps.RpsChoice = null;
        this.DogRps.RpsLocked = false;
        this.DogRps.RpsChoice = null;
    }

    public void StartBulletTime()
    {
        if (this.bulletTimeCoroutine != null)
        {
            StopCoroutine(this.bulletTimeCoroutine);
        }

        this.bulletTimeCoroutine = StartCoroutine(StartBulletTimeImpl());
    }

    private IEnumerator StartBulletTimeImpl()
    {
        this.InBulletTime = true;
        this.dog.PauseAfterRps = true;
        this.bulletTimer = this.scaledSeconds;
        this.RabbitRps.RpsChoice = null;
        this.DogRps.RpsChoice = null;
        UIManager.Instance.HideRpsPanel();
        UIManager.Instance.ShowRpsActions();
        UIManager.Instance.ShowCombatFrame(true);
        UIManager.Instance.ShowRpsTimer(true);

        if (this.showRpsCoroutine != null)
        {
            StopCoroutine(this.showRpsCoroutine);
        }

        SlowTime();
        GameManager.Instance.PlaySound(this.countdownClip);

        yield return new WaitForSeconds(this.scaledSeconds);

        StartCoroutine(FinishRps());
    }

    public void SkipRpsTimer()
    {
        StopCoroutine(this.bulletTimeCoroutine);
        StartCoroutine(FinishRps());
    }

    private IEnumerator FinishRps()
    {
        RestoreTime();

        this.InBulletTime = false;
        UIManager.Instance.BulletTimeText.SetText("");
        UIManager.Instance.ShowCombatFrame(false);
        UIManager.Instance.ShowRpsTimer(false);

        DetermineWinner();

        yield return new WaitForSeconds(this.dog.afterRpsCantAttackTime);

        this.dog.PauseAfterRps = false;
    }

    public void StopRps()
    {
        RestoreTime();
        this.InBulletTime = false;
        UIManager.Instance.BulletTimeText.SetText("");
        UIManager.Instance.ShowCombatFrame(false);
        UIManager.Instance.ShowRpsTimer(false);
        this.dog.PauseAfterRps = false;
    }

    private void DetermineWinner()
    {
        GameManager.Instance.Rabbit.LookAt(GameManager.Instance.Dog);
        GameManager.Instance.Dog.LookAt(GameManager.Instance.Rabbit);

        Character? winner = CheckRps(this.RabbitRps.RpsChoice, this.DogRps.RpsChoice);

        this.showRpsCoroutine = StartCoroutine(ShowRpsChoices(winner));

        if (winner == Character.Dog)
        {
            //this.rabbitAnimator.SetInteger("Lives", --GameManager.Instance.Lives);
            StartCoroutine(PlayAttackAnimations(this.dogRb.gameObject, this.dogAnimator, this.DogRps.RpsChoice.ToString()));
            PersistentData.DeusWins++;
        }
        else if (winner == Character.Rabbit)
        {
            StartCoroutine(PlayAttackAnimations(this.rabbitRb.gameObject, this.rabbitAnimator, this.RabbitRps.RpsChoice.ToString()));
            PersistentData.BrevisWins++;
        }
        else
        {
            this.rabbitMovement.PauseMovement(0.1f);
            this.dogMovement.PauseMovement(0.1f);
            this.rabbitRb.velocity = Vector3.zero;
            this.dogRb.AddForce((GameManager.Instance.Dog.position - GameManager.Instance.Rabbit.position) * this.knockbackAmount, ForceMode.VelocityChange);
            this.rabbitRb.AddForce((GameManager.Instance.Rabbit.position - GameManager.Instance.Dog.position) * this.knockbackAmount, ForceMode.VelocityChange);
        }

        UnlockRpsChoices();
    }

    private void RestoreTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime /= this.BulletTimeScale;
        Time.maximumDeltaTime /= this.BulletTimeScale;
    }

    private void SlowTime()
    {
        Time.timeScale = this.BulletTimeScale;
        Time.fixedDeltaTime *= this.BulletTimeScale;
        Time.maximumDeltaTime *= this.BulletTimeScale;
    }

    // A naive implementation, but this system probably doesn't warrant anything much more complex.
    private Character? CheckRps(Rps? rabbitRps, Rps? dogRps)
    {
        if (rabbitRps == dogRps)
        {
            return null;
        }
        else if (rabbitRps == Rps.Rock)
        {
            return (dogRps == Rps.Paper) ? Character.Dog : Character.Rabbit;
        }
        else if (rabbitRps == Rps.Paper)
        {
            return (dogRps == Rps.Scissors) ? Character.Dog : Character.Rabbit;
        }
        else if (rabbitRps == Rps.Scissors)
        {
            return (dogRps == Rps.Rock) ? Character.Dog : Character.Rabbit;
        }
        
        return Character.Dog;
    }

    private IEnumerator ShowRpsChoices(Character? winner)
    {
        Rps? p1Choice = PersistentData.P1Character == Character.Dog ? this.DogRps.RpsChoice : this.RabbitRps.RpsChoice;
        Rps? p2Choice = PersistentData.P1Character == Character.Dog ? this.RabbitRps.RpsChoice : this.DogRps.RpsChoice;

        UIManager.Instance.BulletTimeText.SetText(string.Empty);
        UIManager.Instance.ShowRpsChoices(p1Choice, p2Choice);

        Player playerWinner = (PersistentData.P1Character == winner) ? Player.Player1 : Player.Player2;
        Player loser = playerWinner == Player.Player1 ? Player.Player2 : Player.Player1;

        if (winner != null)
        {
            UIManager.Instance.ShowAttacks(playerWinner);
            UIManager.Instance.ShowLoseImage(loser, true);
        }
        else
        {
            if (p1Choice != null && p2Choice != null)
            {
                UIManager.Instance.ShowAttacks(null);
            }
        }

        yield return new WaitForSeconds(this.secondsToShowRpsChoices);

        UIManager.Instance.HideRpsPanel();
    }

    private IEnumerator PlayAttackAnimations(GameObject winner, Animator winningAnimator, string winningAnimation)
    {
        var winnerMovement = winner.GetComponent<PlayerMovement>();
        winnerMovement.CanMove = false;
        winningAnimator.SetTrigger(winningAnimation);
        yield return 0; // Tick a single frame to get transition info
        float animationWaitTime = winningAnimator.GetNextAnimatorStateInfo(0).length + winningAnimator.GetAnimatorTransitionInfo(0).duration;
        yield return new WaitForSeconds(animationWaitTime);
        if (winner == this.dogRb.gameObject)
        {
            yield return new WaitForSeconds(Constants.Dog_Howl);
        }
        winnerMovement.CanMove = true;
    }
}
