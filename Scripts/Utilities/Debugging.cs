using UnityEngine;

public class Debugging : MonoBehaviour
{
    private void Start()
    {
        foreach (string item in Input.GetJoystickNames())
        {
            Debug.Log(item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(GameManager.Instance.SetWin(Character.Rabbit));
        }
    }
}
