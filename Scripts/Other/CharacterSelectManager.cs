using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingText;

    [SerializeField]
    private GameObject characterButtonsPanel;

    [SerializeField]
    private Transform brevisImage;

    [SerializeField]
    private Transform deusImage;

    [SerializeField]
    private Transform deusScreenshot;

    [SerializeField]
    private Transform brevisScreenshot;

    private Character p1Character = Character.Rabbit;
    private Character p2Character = Character.Dog;
    private bool switching = false;

    private void Start()
    {
        this.loadingText.SetActive(false);
        MusicPlayer.Instance.PlayTrack(Constants.Music_Menu);
    }

    private void Update()
    {
        if (EventSystem.current == null)
        {
            return;
        }

        if (Mathf.Abs(Input.GetAxis(Constants.Input_P1Horizontal)) > 0f)
        {
            if (!this.switching)
            {
                this.switching = true;
                HelperMethods.Swap(ref this.p1Character, ref this.p2Character);
                SFXPlayer.Instance.PlaySelectSound();

                // Swap character images (won't let me pass properties as ref params, so can't use helper method)
                Vector3 brevisImagePosOrig = this.brevisImage.position;
                this.brevisImage.position = this.deusImage.position;
                this.deusImage.position = brevisImagePosOrig;

                // Swap character screenshots
                Vector3 brevisScreenshotPosOrig = this.brevisScreenshot.position;
                this.brevisScreenshot.position = this.deusScreenshot.position;
                this.deusScreenshot.position = brevisScreenshotPosOrig;
            }
        }
        else
        {
            this.switching = false;
        }

        if (Input.GetButtonDown(Constants.Input_Cancel))
        {
            HelperMethods.Instance.LoadScene(Constants.Scene_Objectives);
        }
    }

    public void OnStartButtonClick()
    {
        SFXPlayer.Instance.PlayConfirmSound();
        PersistentData.P1Character = this.p1Character;
        HelperMethods.Instance.LoadScene(Constants.Scene_Controls);
    }
}
