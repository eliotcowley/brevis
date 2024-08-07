using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public bool TimerDone = false;

    [SerializeField]
    private int timerStart;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float timer;

    private void Start()
    {
        this.timer = this.timerStart;
    }

    private void Update()
    {
        if (this.TimerDone)
        {
            return;
        }

        if (this.timer <= 0f)
        {
            StartCoroutine(GameManager.Instance.SetWin(null));
            this.TimerDone = true;
        }

        this.timer -= Time.deltaTime;
        int minutes = (int)(this.timer / 60);
        int seconds = (int)(this.timer % 60);
        this.timerText.SetText($"Timer: {minutes:D1} : {seconds:D2}");
    }
}
