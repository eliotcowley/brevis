using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(RoundManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public int Carrots = 0;

    [HideInInspector]
    public Dog DogScript;

    [HideInInspector]
    public bool IsPaused = false;

    [HideInInspector]
    public bool GameOver = false;

    [HideInInspector]
    public bool PickUpActive = false;

    [HideInInspector]
    public int AvailableCarrots = 0;

    [HideInInspector]
    public Vector3 RabbitStartingPosition;

    public Transform Rabbit;
    public Transform Dog;
    public int Lives = Constants.StartingLives;

    [SerializeField]
    private GameObject carrotObject;

    [SerializeField]
    private GameObject allCarrots;

    [SerializeField]
    private Transform dropoffPointsContainer;

    [SerializeField]
    private int numDropoffPoints = 3;

    [SerializeField]
    private int numRounds = 2;

    [SerializeField]
    private Transform treesContainer;

    [SerializeField]
    private float minSecondsBeforeNextRound = 0.5f;

    [SerializeField]
    private float battleDistance = 5.0f;

    [SerializeField]
    private AudioClip dogGrowl;

    private Timer timer;
    private bool battleMusic = false;
    private PlayerMovement rabbitMovement;
    private PlayerMovement dogMovement;
    private Rabbit rabbitScript;
    private Animator rabbitAnimator;
    private GameObject[] heldCarrots;
    private float prevTimeScale = 1f;
    private StandaloneInputModule inputModule;
    private Player pausedPlayer = Player.Player1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Random.InitState((int)DateTime.Now.Ticks);
        RandomlySetDropoffPointsInactive();
        this.timer = GetComponent<Timer>();
        this.GameOver = false;
        this.DogScript = this.Dog.GetComponent<Dog>();
        this.rabbitMovement = this.Rabbit.GetComponent<PlayerMovement>();
        this.dogMovement = this.Dog.GetComponent<PlayerMovement>();
        this.rabbitScript = this.Rabbit.GetComponent<Rabbit>();
        this.rabbitAnimator = this.Rabbit.GetComponent<Animator>();
        this.heldCarrots = GameObject.FindGameObjectsWithTag("Carrot");

        foreach(GameObject heldCarrot in this.heldCarrots)
        {
            heldCarrot.SetActive(false);
        }

        if (PersistentData.P1Character == Character.Dog)
        {
            RotateChildren180(this.treesContainer);
            RotateChildren180(this.dropoffPointsContainer);
        }

        MusicPlayer.Instance.PlayTrack(Constants.Music_Level_Theme);
        PersistentData.NumBurrowsInPreviousLevel = this.numDropoffPoints;
        this.inputModule = EventSystem.current.GetComponent<StandaloneInputModule>();
        Application.targetFrameRate = Constants.TargetFrameRate;
        PersistentData.ResetGame();
    }

    private void RotateChildren180(Transform transform)
    {
        Vector3 newRotation = new Vector3(0f, 180f, 0f);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.Rotate(newRotation, Space.Self);
        }
    }

    private void RandomlySetDropoffPointsInactive()
    {
        int dropoffPointsToSetInactive = this.dropoffPointsContainer.childCount - this.numDropoffPoints;
        List<Transform> dropoffPointsList = new List<Transform>();

        for (int i = 0; i < this.dropoffPointsContainer.childCount; i++)
        {
            dropoffPointsList.Add(this.dropoffPointsContainer.GetChild(i));
        }

        for (int i = 0; i < dropoffPointsToSetInactive; i++)
        {
            int randomDropoffPoint = Random.Range(0, dropoffPointsList.Count);
            dropoffPointsList[randomDropoffPoint].gameObject.SetActive(false);
            dropoffPointsList.RemoveAt(randomDropoffPoint);
        }
    }

    private void Update()
    {
        if (EventSystem.current == null)
        {
            return;
        }

        if (Vector3.Distance(this.Rabbit.position, this.Dog.position) <= this.battleDistance && !this.battleMusic)
        {
            this.battleMusic = true;
            PlaySound(this.dogGrowl, 0.3f);
        }
        else if (Vector3.Distance(this.Rabbit.position, this.Dog.position) > this.battleDistance && this.battleMusic)
        {
            this.battleMusic = false;
        }

        if (Input.GetButtonDown(Constants.Input_PauseP1))
        {
            if (!(this.IsPaused && this.pausedPlayer == Player.Player2))
            {
                UIManager.Instance.SetPauseMenuImage(Player.Player1);
                UIManager.Instance.TogglePauseMenu();
                this.inputModule.verticalAxis = Constants.Input_P1Vertical;
                this.inputModule.submitButton = Constants.Input_P1Primary;
                this.pausedPlayer = Player.Player1;
            }
        }

        if (Input.GetButtonDown(Constants.Input_PauseP2))
        {
            if (!(this.IsPaused && this.pausedPlayer == Player.Player1))
            {
                UIManager.Instance.SetPauseMenuImage(Player.Player2);
                UIManager.Instance.TogglePauseMenu();
                this.inputModule.verticalAxis = Constants.Input_P2Vertical;
                this.inputModule.submitButton = Constants.Input_P2Primary;
                this.pausedPlayer = Player.Player2;
            }
        }
    }

    public int GetCarrots()
    {
        return this.Carrots;
    }

    public void SetCarrots(int carrots)
    {
        this.Carrots = carrots;
        UIManager.Instance.CarrotsText.SetText($"{Constants.String_Carrots}: {this.Carrots}");

        for (int i = 0; i < this.heldCarrots.Length; i++)
        {
            this.heldCarrots[i].SetActive(i < carrots);
        }
    }

    public IEnumerator SetWin(Character? winningCharacter)
    {
        if (winningCharacter == Character.Rabbit)
        {
            this.Dog.gameObject.SetActive(false);
            this.rabbitAnimator.SetTrigger(Constants.Anim_Win);
        }
        else if (winningCharacter == Character.Dog)
        {
            this.rabbitMovement.CanMove = false;
            this.rabbitScript.Dead = true;
        }
        else
        {
            this.dogMovement.PauseMovement();
            this.rabbitMovement.PauseMovement();
        }

        UIManager.Instance.WinText.SetText(Constants.String_RoundOver);
        this.timer.TimerDone = true;
        PersistentData.RoundTime = Time.timeSinceLevelLoad;
        UIManager.Instance.BulletTimeText.SetText(string.Empty);
        this.GameOver = true;

        yield return new WaitForSeconds(Constants.TransitionToScoresTime);

        PersistentData.PreviousLevel = SceneManager.GetActiveScene().name;
        HelperMethods.Instance.LoadScene(Constants.Scene_Scores);
    }

    public void DropAllCarrots()
    {
        for(int i = 0; i < this.Carrots; i++)
        {
            CreateCarrot(this.Rabbit.position, Constants.Carrot_Blink, Constants.Carrot_Hit_Delay, true);
        }

        SetCarrots(0);
        PersistentData.CarrotsInPack = 0;
    }

    public void CreateCarrot(Vector3 origin, float ejectHeight, float delayDuration, bool attacked = false)
    {
        // Calculate random angle to throw carrot
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        // Throw carrot
        var carrot = Instantiate(this.carrotObject, new Vector3(origin.x, origin.y + ejectHeight, origin.z), Quaternion.identity);
        carrot.transform.SetParent(this.allCarrots.transform);
        carrot.GetComponent<Rigidbody>().AddForce(x, 3, z, ForceMode.Impulse);
        BlinkObject(carrot, Constants.Carrot_Blink, 0.2f, delayDuration, true, attacked);
    }

    public IEnumerator SafeDestroy(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(obj);
    }

    public void BlinkObject (GameObject obj, float blinkDuration, float blinkTime, float delayDuration = 0, bool destroy = false, bool attacked = false)
    {
        StartCoroutine(Blink(obj, blinkDuration, blinkTime, delayDuration, destroy, attacked));
    }

    private IEnumerator Blink(GameObject obj, float blinkDuration, float blinkTime, float delayDuration, bool destroy, bool attacked = false)
    {
        var renderer = obj.GetComponentInChildren<Renderer>();
        Carrot carrotScript = obj.GetComponent<Carrot>();

        yield return new WaitForSeconds(delayDuration);

        while (blinkDuration > 0)
        {
            blinkDuration -= blinkTime;

            // Always render if currently being picked up; otherwise, toggle rendering.
            renderer.enabled = ((carrotScript != null) && carrotScript.PickedUp) || !renderer.enabled;

            yield return new WaitForSeconds(blinkTime);
        }
        if (destroy)
        {
            Destroy(obj);

            if (attacked)
            {
                PersistentData.CarrotsReturned++;
            }
            this.AvailableCarrots--;
        }
        else
        {
            renderer.enabled = true;
        }
    }

    public void CompleteDeliveryPoint()
    {
        this.rabbitAnimator.SetTrigger(Constants.Anim_Deliver);

        if (this.AvailableCarrots == 0)
        {
            StartCoroutine(SetWin(Character.Rabbit));
        }
    }

    public void AlertDog()
    {
        this.DogScript.Track();
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        var listenerPosition = PersistentData.P1Character == Character.Rabbit ? this.Rabbit.position : this.Dog.position;
        AudioSource.PlayClipAtPoint(clip, listenerPosition, volume);
    }

    public void TogglePause()
    {
        this.IsPaused = !this.IsPaused;

        if (this.IsPaused)
        {
            this.prevTimeScale = Time.timeScale;
        }

        Time.timeScale = this.IsPaused ? 0f : this.prevTimeScale;
    }

    public void TakeLife()
    {
        this.Lives--;

        if (this.Lives > 0)
        {
            this.Rabbit.position = this.RabbitStartingPosition;
            this.rabbitAnimator.SetTrigger(Constants.Anim_Respawn);
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetWin(Character.Dog));
        }

        UIManager.Instance.UpdateLives();
    }
}
