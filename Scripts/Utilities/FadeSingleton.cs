using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Prefab("FadeImageCanvas")]
public class FadeSingleton : Singleton<FadeSingleton>
{
    [SerializeField]
    private FadeImage fadeImage;

    [SerializeField]
    private GameObject loadingImage;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void FadeIn(float fadeTime)
    {
        this.fadeImage.FadeIn(fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        this.fadeImage.FadeOut(fadeTime);
    }

    public void FadeOutImmediate(float fadeTime)
    {
        this.fadeImage.FadeOut(fadeTime, true);
    }

    public void ShowLoadingText(bool show)
    {
        this.loadingImage.SetActive(show);
    }
}
