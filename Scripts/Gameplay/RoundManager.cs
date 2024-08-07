using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private CameraFollow p1Camera;

    [SerializeField]
    private CameraFollow p2Camera;

    [SerializeField]
    private TextMeshProUGUI roundText;

    private bool swap;

    private void Awake()
    {
        this.swap = PersistentData.P1Character == Character.Dog;
        SetCameraTargets();
        this.roundText.SetText($"{Constants.String_Round}: {PersistentData.Round}");
    }

    public void NextRound()
    {
        PersistentData.P1Character = (PersistentData.P1Character == Character.Rabbit) ? Character.Dog : Character.Rabbit;
        PersistentData.Round++;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetCameraTargets()
    {
        Transform p1CameraTransform = this.p1Camera.transform;
        Transform p2CameraTransform = this.p2Camera.transform;

        if (this.swap)
        {
            HelperMethods.Swap(ref this.p1Camera.Target, ref this.p2Camera.Target);
            p1CameraTransform.Rotate(new Vector3(0f, -180f, 0f), Space.World);
            p2CameraTransform.Rotate(new Vector3(0f, -180f, 0f), Space.World);
        }
    }
}
