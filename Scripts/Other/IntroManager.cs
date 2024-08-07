using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class IntroManager : MonoBehaviour
{
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

    [SerializeField]
    private FadeImage skipImageFade;

    [SerializeField]
    private Image skipProgressImage;

    [SerializeField]
    private float skipTime = 1f;

    [SerializeField]
    private TextMeshProUGUI narrativeText;

    [SerializeField]
    private string[] narrativeStrings;

    [SerializeField]
    private float sfxDelay = 3f;

    private int index = 0;
    private bool isSkipVisible = false;
    private bool skipTimerStarted = false;
    private float skipTimer = 0f;
    private bool skipping = false;
    private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(FadeInAndOutImages());
        MusicPlayer.Instance.PlayTrack(Constants.Music_Menu);
        this.audioSource = GetComponent<AudioSource>();
        this.audioSource.PlayDelayed(this.sfxDelay);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!this.isSkipVisible)
            {
                this.isSkipVisible = true;
                this.skipImageFade.FadeIn(this.fadeTime);
            }

            if (!this.skipTimerStarted)
            {
                this.skipTimerStarted = true;
            }
        }

        if (Input.GetButton(Constants.Input_Submit))
        {
            if (this.skipTimerStarted && !this.skipping)
            {
                this.skipTimer += Time.deltaTime;

                if (this.skipTimer >= this.skipTime)
                {
                    StopCoroutine(FadeInAndOutImages());
                    StartCoroutine(SkipIntro());
                    this.skipTimer = 0f;
                    this.skipping = true;
                }
            }
        }
        else
        {
            if (this.skipTimerStarted && !this.skipping)
            {
                this.skipTimerStarted = false;
                this.skipTimer = 0f;
            }
        }

        this.skipProgressImage.fillAmount = this.skipTimer / this.skipTime;
    }

    private IEnumerator FadeInAndOutImages()
    {
        yield return new WaitForSeconds(this.blackoutTime);

        this.fadeBlack.FadeOut(this.fadeTime);

        while (this.index < this.sceneImages.Length)
        {
            this.sceneImages[this.index].FadeIn(this.fadeTime);
            this.narrativeText.SetText(this.narrativeStrings[this.index]);

            yield return new WaitForSeconds(this.fadeTime);
            yield return new WaitForSeconds(this.sceneTime);

            this.sceneImages[this.index++].FadeOut(this.fadeTime);
        }

        this.fadeBlack.FadeIn(this.fadeTime, true);
        yield return new WaitForSeconds(this.fadeTime);
        SceneManager.LoadScene(Constants.Scene_Title);
    }

    private IEnumerator SkipIntro()
    {
        this.fadeBlack.FadeIn(this.fadeTime, true);
        this.skipImageFade.FadeOut(this.fadeTime);
        yield return new WaitForSeconds(this.fadeTime);
        SceneManager.LoadScene(Constants.Scene_Title);
    }
}
