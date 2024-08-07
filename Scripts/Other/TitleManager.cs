using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;

    private void Start()
    {
        MusicPlayer.Instance.PlayTrack(Constants.Music_Menu, 0.3f);
    }

    private void Update()
    {
        if (EventSystem.current == null)
        {
            return;
        }

        if (Input.GetButtonDown(Constants.Input_Cancel))
        {
            HelperMethods.QuitApp();
        }
    }

    public void OnStartGameButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        HelperMethods.Instance.LoadScene(Constants.Scene_Objectives);
    }

    public void OnQuitButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        HelperMethods.QuitApp();
    }

    public void OnCreditsButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        HelperMethods.Instance.LoadScene(Constants.Scene_Credits);
    }
}
