using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;

    [SerializeField]
    private FadeImage[] sceneImages;

    [SerializeField]
    private FadeImage fadeBlack;

    [SerializeField]
    private float sceneTime = 3f;

    [SerializeField]
    private float fadeTime = 1f;

    [SerializeField]
    private float blackoutTime = 1f;


    private int index = 0;
    private bool playTransitions = false;

    private void Start()
    {
        this.playTransitions = true;
        StartCoroutine(FadeInAndOutImages());
    }

    private IEnumerator FadeInAndOutImages()
    {
        yield return new WaitForSeconds(this.blackoutTime);

        this.fadeBlack.FadeOut(this.fadeTime);

        while (this.playTransitions)
        {
            this.sceneImages[this.index].FadeIn(this.fadeTime);

            yield return new WaitForSeconds(this.fadeTime + this.sceneTime);

            this.sceneImages[this.index++].FadeOut(this.fadeTime);

            this.index %= this.sceneImages.Length;
        }
    }

    public void OnGoHomeButtonPressed()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        this.playTransitions = false;
        HelperMethods.Instance.LoadScene(Constants.Scene_Title);
    }
}
