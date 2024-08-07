using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Prefab("FpsCounter")]
public class FpsCounter : Singleton<FpsCounter>
{
    [SerializeField]
    private float refreshTime = 0.5f;

    private TextMeshProUGUI text;
    private int frameCount = 0;
    private float timer = 0f;
    private int lastFps = 0;

    private void Start()
    {
        this.text = GetComponentInChildren<TextMeshProUGUI>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (this.timer < this.refreshTime)
        {
            this.timer += Time.deltaTime;
            this.frameCount++;
        }
        else
        {
            this.lastFps = (int)(this.frameCount / this.timer);
            this.frameCount = 0;
            this.timer = 0f;
        }

        this.text.SetText($"FPS: {this.lastFps}");
    }

    /// <summary>
    /// Dummy method whose sole purpose is to activate the singleton prefab.
    /// </summary>
    public void Dummy()
    {
        Debug.Log("Started FPS counter");
    }
}
