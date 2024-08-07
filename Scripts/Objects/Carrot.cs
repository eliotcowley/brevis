using System.Collections;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    [HideInInspector]
    public bool PickedUp = false;

    [SerializeField]
    private float timeBeforeDisappearWhenPickedUp = 1f;

    private Animator rabbitAnimator;
    private PlayerMovement rabbitMovement;
    private Lerp rabbitLerp;
    private float rotateTowardsCarrotTime;

    private void Start()
    {
        this.rabbitAnimator = GameManager.Instance.Rabbit.GetComponent<Animator>();
        this.rabbitMovement = GameManager.Instance.Rabbit.GetComponent<PlayerMovement>();
        this.rabbitLerp = GameManager.Instance.Rabbit.GetComponent<Lerp>();
        this.rotateTowardsCarrotTime = GameManager.Instance.Rabbit.GetComponent<Rabbit>().RotateTowardsCarrotTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Rabbit))
        {
            if (!this.PickedUp)
            {
                this.PickedUp = true;
                this.rabbitLerp.LerpRotateTowardsObject(other.gameObject.transform, this.transform, this.rotateTowardsCarrotTime);

                if (!GameManager.Instance.PickUpActive)
                {
                    GameManager.Instance.PickUpActive = true;
                    this.rabbitAnimator.SetTrigger(Constants.Anim_Pickup);
                }

                StartCoroutine(DisappearCarrot());
                GameManager.Instance.SetCarrots(GameManager.Instance.GetCarrots() + 1);
                PersistentData.CarrotsInPack = GameManager.Instance.GetCarrots();
                this.rabbitMovement.PauseMovement(Constants.Rabbit_PickupTime);
            }
        }
    }

    private IEnumerator DisappearCarrot()
    {
        yield return new WaitForSeconds(this.timeBeforeDisappearWhenPickedUp);
        this.gameObject.SetActive(false);
    }
}
