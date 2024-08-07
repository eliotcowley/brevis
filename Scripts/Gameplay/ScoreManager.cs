using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;

    [SerializeField]
    private TextMeshProUGUI brevisText;

    [SerializeField]
    private TextMeshProUGUI deusText;

    [SerializeField]
    private TextMeshProUGUI carrotsStolenNumText;

    [SerializeField]
    private TextMeshProUGUI burrowsUsedNumText;

    [SerializeField]
    private TextMeshProUGUI matchTimeText;

    [SerializeField]
    private TextMeshProUGUI brevisCombatWinsText;

    [SerializeField]
    private TextMeshProUGUI deusCombatWinsText;

    public void OnTellAnotherTaleButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        PersistentData.ResetGame();
        HelperMethods.Instance.LoadScene(Constants.Scene_CharacterSelectScene);
    }

    public void OnGoHomeButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        HelperMethods.Instance.LoadScene(Constants.Scene_Title);
    }

    private void Start()
    {
        this.loadingText.SetActive(false);

        int brevisPlayer = PersistentData.P1Character == Character.Rabbit ? 1 : 2;
        this.brevisText.SetText($"P{brevisPlayer}");

        int deusPlayer = brevisPlayer == 1 ? 2 : 1;
        this.deusText.SetText($"P{deusPlayer}");

        this.carrotsStolenNumText.SetText(PersistentData.CarrotsStolen.ToString());

        int minutes = (int)(PersistentData.RoundTime / 60);
        int seconds = (int)(PersistentData.RoundTime % 60);
        this.matchTimeText.SetText($"{minutes} : {seconds.ToString("00")}");

        this.brevisCombatWinsText.SetText(PersistentData.BrevisWins.ToString());
        this.deusCombatWinsText.SetText(PersistentData.DeusWins.ToString());
    }

    private void Update()
    {
        if (EventSystem.current == null)
        {
            return;
        }

        if (Input.GetButtonDown(Constants.Input_Cancel))
        {
            HelperMethods.Instance.LoadScene(Constants.Scene_Title);
        }
    }
}
