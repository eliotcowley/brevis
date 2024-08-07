using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImage : MonoBehaviour
{
    private float fadeTime = 1f;
    private Fade fadeState = Fade.None;
    private Image image;
    private Color fadeInColor;
    private Color fadeOutColor;
    private float timer = 0f;
    private Color origColor;

    public void FadeIn(float fadeTime, bool immediate = false)
    {
        this.fadeTime = fadeTime;
        this.origColor = this.image.color;
        this.timer = 0f;

        if (immediate)
        {
            this.fadeState = Fade.FadeInImmediate;
        }
        else
        {
            this.fadeState = Fade.FadeIn;
        }
    }

    public void FadeOut(float fadeTime, bool immediate = false)
    {
        this.fadeTime = fadeTime;
        this.origColor = this.image.color;
        this.timer = 0f;

        if (immediate)
        {
            this.fadeState = Fade.FadeOutImmediate;
        }
        else
        {
            this.fadeState = Fade.FadeOut;
        }
    }

    private void Awake()
    {
        this.image = GetComponent<Image>();
        this.origColor = this.image.color;
        this.fadeInColor = new Color(this.origColor.r, this.origColor.g, this.origColor.b, 1f);
        this.fadeOutColor = new Color(this.origColor.r, this.origColor.g, this.origColor.b, 0f);
    }

    private void Update()
    {
        switch (this.fadeState)
        {
            case Fade.None:
                break;
            case Fade.FadeIn:
                this.image.color = Color.Lerp(this.fadeOutColor, this.fadeInColor, this.timer / this.fadeTime);
                this.timer += Time.deltaTime;
                break;
            case Fade.FadeOut:
                this.image.color = Color.Lerp(this.fadeInColor, this.fadeOutColor, this.timer / this.fadeTime);
                this.timer += Time.deltaTime;
                break;
            case Fade.FadeInImmediate:
                this.image.color = Color.Lerp(this.origColor, this.fadeInColor, this.timer / this.fadeTime);
                this.timer += Time.deltaTime;
                break;
            case Fade.FadeOutImmediate:
                this.image.color = Color.Lerp(this.origColor, this.fadeOutColor, this.timer / this.fadeTime);
                this.timer += Time.deltaTime;
                break;
            default:
                break;
        }

        if (this.timer >= this.fadeTime)
        {
            if (this.fadeState == Fade.FadeIn)
            {
                this.image.color = this.fadeInColor;
            }
            else if (this.fadeState == Fade.FadeOut)
            {
                this.image.color = this.fadeOutColor;
            }

            this.fadeState = Fade.None;
            this.timer = 0f;
        }
    }
}

public enum Fade
{
    None,
    FadeIn,
    FadeOut,
    FadeInImmediate,
    FadeOutImmediate
}