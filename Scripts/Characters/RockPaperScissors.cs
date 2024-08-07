using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class RockPaperScissors : MonoBehaviour
{
    [HideInInspector]
    public bool RpsLocked = false;

    [HideInInspector]
    public Rps? RpsChoice;

    private PlayerMovement playerMovement;
    private string inputRock;
    private string inputPaper;
    private string inputScissors;
    
    private void Start()
    {
        this.playerMovement = GetComponent<PlayerMovement>();
        this.RpsChoice = null;

        if (this.playerMovement.PlayerCharacter == Character.Rabbit)
        {
            this.inputRock = InputManager.RabbitRock;
            this.inputPaper = InputManager.RabbitPaper;
            this.inputScissors = InputManager.RabbitScissors;
        }
        else
        {
            this.inputRock = InputManager.DogRock;
            this.inputPaper = InputManager.DogPaper;
            this.inputScissors = InputManager.DogScissors;
        }
    }

    private void Update()
    {
        if (RpsManager.Instance.InBulletTime && !GameManager.Instance.IsPaused)
        {
            if (!this.RpsLocked)
            {
                if (Input.GetButtonDown(this.inputRock))
                {
                    LockInChoice(Rps.Rock);
                }
                else if (Input.GetButtonDown(this.inputPaper))
                {
                    LockInChoice(Rps.Paper);
                }
                else if (Input.GetButtonDown(this.inputScissors))
                {
                    LockInChoice(Rps.Scissors);
                }
            }
        }
    }

    private void LockInChoice(Rps choice)
    {
        this.RpsLocked = true;
        this.RpsChoice = choice;
        UIManager.Instance.ShowCheckmark(this.playerMovement.Player);

        if (RpsManager.Instance.RabbitRps.RpsLocked && RpsManager.Instance.DogRps.RpsLocked)
        {
            RpsManager.Instance.SkipRpsTimer();
        }
    }
}

public enum Rps
{
    Rock,
    Paper,
    Scissors
}
