using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static Character P1Character = Character.Rabbit;
    public static int Round = 1;
    public static string PreviousLevel = "";
    public static string NextLevel = "";
    public static int CarrotsStolen = 0;
    public static int CarrotsInPack = 0;
    public static int CarrotsReturned = 0;
    public static int NumBurrowsInPreviousLevel = 0;
    public static float RoundTime = 0f;
    public static int BrevisWins = 0;
    public static int DeusWins = 0;

    public static bool IsP1Joystick 
    {
        get
        {
            string[] joystickNames = Input.GetJoystickNames();

            foreach (string item in joystickNames)
            {
                if (item.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static bool IsP2Joystick 
    {
        get
        {
            int numJoysticks = 0;
            string[] joystickNames = Input.GetJoystickNames();

            foreach (string item in joystickNames)
            {
                if (item.Length > 0)
                {
                    numJoysticks++;

                    if (numJoysticks > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static void ResetGame()
    {
        //P1Character = Character.Rabbit;
        Round = 1;
        Time.timeScale = 1f;
        CarrotsStolen = 0;
        CarrotsInPack = 0;
        CarrotsReturned = 0;
        RoundTime = 0f;
        BrevisWins = 0;
        DeusWins = 0;
    }
}
