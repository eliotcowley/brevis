using TMPro;
using UnityEngine;

public class DropoffPoint : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dropoffText;

    [SerializeField]
    private Light burrowLight;

    private bool rabbitInside = false;
    private string interactButton;
    private PlayerMovement rabbitMovement;

    private void Start()
    {
        this.dropoffText.gameObject.SetActive(false);
        this.interactButton = PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Primary : Constants.Input_P2Primary;
        this.dropoffText.SetText(Constants.String_A);
        this.rabbitMovement = GameManager.Instance.Rabbit.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(this.interactButton) && this.rabbitInside)
        {
            if (GameManager.Instance.Carrots > 0)
            {
                int rabbitCarrots = GameManager.Instance.GetCarrots();
                GameManager.Instance.SetCarrots(0);

                PersistentData.CarrotsStolen += rabbitCarrots;
                PersistentData.CarrotsInPack = 0;

                GameManager.Instance.CompleteDeliveryPoint();
                this.rabbitMovement.PauseMovement(Constants.Rabbit_DropoffTime);
                this.dropoffText.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Rabbit))
        {
            if (GameManager.Instance.Carrots > 0)
            {
                this.dropoffText.gameObject.SetActive(true);
            }
            
            this.rabbitInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Rabbit))
        {
            this.dropoffText.gameObject.SetActive(false);
            this.rabbitInside = false;
        }
    }
}
