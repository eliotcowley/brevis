using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HelperMethods : Singleton<HelperMethods>
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void Swap<T>(ref T object1, ref T object2)
    {
        T object1Orig = object1;
        object1 = object2;
        object2 = object1Orig;
    }

    public static void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Gets the string representation of the primary input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose primary input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetPrimaryKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_A : Constants.String_Q;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_A : Constants.String_J;
        }
    }

    /// <summary>
    /// Gets the string representation of the secondary input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose secondary input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetSecondaryKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_X : Constants.String_E;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_X : Constants.String_K;
        }
    }

    /// <summary>
    /// Gets the string representation of the special input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose special input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetSpecialKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_Y : Constants.String_R;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_Y : Constants.String_L;
        }
    }

    /// <summary>
    /// Gets the string representation of the rock input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose rock input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetRockKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_A : Constants.String_1;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_A : Constants.String_U;
        }
    }

    /// <summary>
    /// Gets the string representation of the paper input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose paper input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetPaperKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_X : Constants.String_2;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_X : Constants.String_I;
        }
    }

    /// <summary>
    /// Gets the string representation of the scissors input, given the player number.
    /// </summary>
    /// <param name="playerNum">Number of the player whose scissors input to return (zero-based).</param>
    /// <returns></returns>
    public static string GetScissorsKey(int playerNum)
    {
        if (playerNum == 0)
        {
            return PersistentData.IsP1Joystick ? Constants.String_Y : Constants.String_3;
        }
        else
        {
            return PersistentData.IsP2Joystick ? Constants.String_Y : Constants.String_O;
        }
    }

    public void LoadScene(string scene, bool fade = true, bool showLoadingText = false)
    {
        if (fade)
        {
            StartCoroutine(LoadSceneFadeCoroutine(scene, showLoadingText));
        }
        else
        {
            if (showLoadingText)
            {
                FadeSingleton.Instance.ShowLoadingText(true);
            }

            SceneManager.LoadScene(scene);
            FadeSingleton.Instance.ShowLoadingText(false);
        }
    }

    public void LoadScene(string scene, GameObject loadingText, bool fade = true)
    {
        loadingText.SetActive(true);
        LoadScene(scene, fade, false);
    }

    public void LoadSceneFadeInOnly(string scene, GameObject loadingText)
    {
        StartCoroutine(LoadSceneFadeInOnlyCoroutine(scene, loadingText));
    }

    private IEnumerator LoadSceneFadeInOnlyCoroutine(string scene, GameObject loadingText)
    {
        loadingText.SetActive(true);
        SceneManager.LoadScene(scene);

        yield return new WaitForEndOfFrame();

        FadeSingleton.Instance.FadeOut(Constants.SceneFadeTime);

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator LoadSceneFadeCoroutine(string scene, bool showLoadingText)
    {
        FadeSingleton.Instance.FadeIn(Constants.SceneFadeTime);
        EventSystem.current.enabled = false;

        yield return new WaitForSeconds(Constants.SceneFadeTime);

        if (showLoadingText)
        {
            FadeSingleton.Instance.ShowLoadingText(true);
        }

        SceneManager.LoadScene(scene);
        FadeSingleton.Instance.FadeOut(Constants.SceneFadeTime);

        yield return new WaitForEndOfFrame();

        FadeSingleton.Instance.ShowLoadingText(false);
    }
}
