using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public TextMeshProUGUI CarrotsText;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI RestartText;
    public TextMeshProUGUI BulletTimeText;

    [SerializeField]
    private TextMeshProUGUI carrotsInPackText;

    [SerializeField]
    private TextMeshProUGUI deliveredText;

    [SerializeField]
    private TextMeshProUGUI returnedText;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private Selectable resumeButton;

    [SerializeField]
    private GameObject rpsPanel;

    [SerializeField]
    private GameObject p1AttackControls;

    [SerializeField]
    private GameObject p2AttackControls;

    [SerializeField]
    private Image p1AttackSelection;

    [SerializeField]
    private Image p2AttackSelection;

    [SerializeField]
    private Sprite checkmarkSprite;

    [SerializeField]
    private Sprite xSprite;

    [SerializeField]
    private GameObject attackImage;

    [SerializeField]
    private Sprite[] brevisAttackSprites;

    [SerializeField]
    private Sprite[] deusAttackSprites;

    [SerializeField]
    private Sprite p1PauseMenu;

    [SerializeField]
    private Sprite p2PauseMenu;

    [SerializeField]
    private GameObject combatFrame;

    [SerializeField]
    private GameObject p1LoseImage;

    [SerializeField]
    private GameObject p2LoseImage;

    [SerializeField]
    private GameObject rpsTimer;

    [SerializeField]
    private GameObject brevisLivesPanel;

    [SerializeField]
    private Sprite brevisLifeSprite;

    private Image pauseMenuImage;
    private Shake p1AttackShake;
    private Shake p2AttackShake;
    private Slider rpsTimerSlider;
    private Image[] p1AttackControlImages;
    private Image[] p2AttackControlImages;
    private Image[] brevisLivesImages;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("UIManager already exists");
        }
    }

    private void Start()
    {
        this.WinText.SetText("");
        this.pauseMenu.SetActive(false);
        this.pauseMenuImage = this.pauseMenu.GetComponent<Image>();
        this.p1AttackShake = this.p1AttackSelection.GetComponent<Shake>();
        this.p2AttackShake = this.p2AttackSelection.GetComponent<Shake>();
        this.rpsTimerSlider = this.rpsTimer.GetComponent<Slider>();
        this.p1AttackControlImages = this.p1AttackControls.GetComponentsInChildren<Image>();
        this.p2AttackControlImages = this.p2AttackControls.GetComponentsInChildren<Image>();

        for (int i = 0; i < this.p1AttackControlImages.Length; i++)
        {
            this.p1AttackControlImages[i].sprite = 
                (PersistentData.P1Character == Character.Rabbit) ? this.brevisAttackSprites[i] : this.deusAttackSprites[i];

            this.p2AttackControlImages[i].sprite =
                (PersistentData.P1Character == Character.Rabbit) ? this.deusAttackSprites[i] : this.brevisAttackSprites[i];
        }

        this.brevisLivesImages = this.brevisLivesPanel.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        this.carrotsInPackText.SetText($"{PersistentData.CarrotsInPack}");
        this.deliveredText.SetText($"{PersistentData.CarrotsStolen}");
        this.returnedText.SetText($"{PersistentData.CarrotsReturned}");
    }

    public void TogglePauseMenu()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        GameManager.Instance.TogglePause();
        this.pauseMenu.SetActive(!this.pauseMenu.activeSelf);
        EventSystem.current.SetSelectedGameObject(null);
        this.resumeButton.Select();
    }

    public void SetPauseMenuImage(Player player)
    {
        this.pauseMenuImage.sprite = (player == Player.Player1) ? this.p1PauseMenu : this.p2PauseMenu;
    }

    public void OnQuitButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        Time.timeScale = 1f;
        HelperMethods.Instance.LoadScene(Constants.Scene_Title);
    }

    public void ShowRpsActions()
    {
        this.rpsPanel.SetActive(true);
        this.p1AttackControls.SetActive(true);
        this.p2AttackControls.SetActive(true);
    }

    public void ShowCheckmark(Player player)
    {
        if (player == Player.Player1)
        {
            this.p1AttackControls.SetActive(false);
            this.p1AttackSelection.enabled = true;
            this.p1AttackSelection.sprite = this.checkmarkSprite;
        }
        else
        {
            this.p2AttackControls.SetActive(false);
            this.p2AttackSelection.enabled = true;
            this.p2AttackSelection.sprite = this.checkmarkSprite;
        }
    }

    public void ShowRpsChoices(Rps? p1Choice, Rps? p2Choice)
    {
        Sprite[] p1Sprites = PersistentData.P1Character == Character.Rabbit ? this.brevisAttackSprites : this.deusAttackSprites;
        Sprite[] p2Sprites = PersistentData.P1Character == Character.Rabbit ? this.deusAttackSprites : this.brevisAttackSprites;

        this.p1AttackSelection.sprite = p1Choice == null ? this.xSprite : p1Sprites[(int)p1Choice];
        this.p2AttackSelection.sprite = p2Choice == null ? this.xSprite : p2Sprites[(int)p2Choice];

        this.p1AttackControls.SetActive(false);
        this.p2AttackControls.SetActive(false);

        this.p1AttackSelection.enabled = true;
        this.p2AttackSelection.enabled = true;

        this.p1AttackShake.Shake2D();
        this.p2AttackShake.Shake2D();
    }

    public void HideAttackImage()
    {
        this.attackImage.SetActive(false);
    }

    public void HideRpsPanel()
    {
        this.p1AttackSelection.enabled = false;
        this.p2AttackSelection.enabled = false;
        this.rpsPanel.SetActive(false);
        this.p1LoseImage.SetActive(false);
        this.p2LoseImage.SetActive(false);
        this.attackImage.SetActive(false);
    }

    public void ShowAttacks(Player? winner)
    {
        if (winner != null)
        {
            this.attackImage.SetActive(true);
            this.attackImage.transform.localScale = (winner == Player.Player1) ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        }
    }

    public void ShowCombatFrame(bool show)
    {
        this.combatFrame.SetActive(show);
    }

    public void ShowLoseImage(Player player, bool show)
    {
        if (player == Player.Player1)
        {
            this.p1LoseImage.SetActive(show);
        }
        else
        {
            this.p2LoseImage.SetActive(show);
        }
    }

    public void ShowRpsTimer(bool show)
    {
        this.rpsTimer.SetActive(show);
    }

    public void SetRpsTimer(float value)
    {
        this.rpsTimerSlider.value = value;
    }

    public void UpdateLives()
    {
        for (int i = 0; i < this.brevisLivesImages.Length; i++)
        {
            this.brevisLivesImages[i].sprite = (GameManager.Instance.Lives + i >= Constants.StartingLives) ? this.brevisLifeSprite : this.xSprite;
        }
    }
}

public enum Player
{
    Player1,
    Player2
}
