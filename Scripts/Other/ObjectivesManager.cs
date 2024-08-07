using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;

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

    public void OnContinueButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        HelperMethods.Instance.LoadScene(Constants.Scene_CharacterSelectScene);
    }
}
