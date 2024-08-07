using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Lerp))]
public class Tree : MonoBehaviour
{
    [SerializeField]
    private int numberOfCarrots = 0;
    [SerializeField]
    private GameObject deadTree;
    [SerializeField]
    private GameObject _pickup;

    [SerializeField]
    private GameObject carrotInBasketPrefab;

    [SerializeField]
    private Vector3 basketPoint = new Vector3(-0.9f, 6.3f, 0.01f);

    [SerializeField]
    private float carrotEjectHeight = 0f;

    [SerializeField]
    private Rigidbody basketRb;

    [SerializeField]
    private float basketForce;

    [SerializeField]
    private float rabbitRotateTowardTreeTime = 0.2f;

    [SerializeField]
    private float rabbitMoveTowardTreeTime = 0.2f;

    [SerializeField]
    private float rabbitShakeTreeTime = 2f;

    [SerializeField]
    private float distanceFromTreeMultiplier = 0.3f;

    private bool _canPickup = false, _pickupOkay = true, _treeChecked = false;
    private readonly float pickupWait = 2.0f;
    private Animator rabbitAnimator;
    private readonly List<GameObject> carrots = new List<GameObject>();
    private GameObject rabbit;
    private PlayerMovement rabbitMovement;
    private Lerp lerpScript;

    private void Start()
    {
        this.rabbitAnimator = GameManager.Instance.Rabbit.GetComponent<Animator>();

        for (int i = 0; i < this.numberOfCarrots; i++)
        {
            GameObject carrot = Instantiate(this.carrotInBasketPrefab, this.transform, false);
            carrot.transform.localPosition = this.basketPoint;
            this.carrots.Add(carrot);
        }

        this.rabbit = GameObject.FindGameObjectWithTag("Rabbit");
        this.rabbitMovement = this.rabbit.GetComponent<PlayerMovement>();
        this.lerpScript = GetComponent<Lerp>();

        GameManager.Instance.AvailableCarrots += this.numberOfCarrots;
    }

    public void CreateCarrot()
    {
        // Remove carrot from basket
        GameObject carrot = this.carrots.First();
        this.carrots.Remove(carrot);
        carrot.SetActive(false);

        // Calculate random angle to throw carrot
        GameManager.Instance.CreateCarrot(this.transform.TransformPoint(new Vector3(this.basketPoint.x, 0.1f, this.basketPoint.z)), this.carrotEjectHeight, Constants.Carrot_Tree_Delay);
        this.numberOfCarrots--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Rabbit))
        {
            this._pickup.SetActive(true);
            this._canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Rabbit))
        {
            this._pickup.SetActive(false);
            this._canPickup = false;
        }
    }

    private void FixedUpdate()
    {
        if(this.numberOfCarrots == 0 && this._treeChecked)
        {
            this._pickup.SetActive(false);
            // Once we get a new empty tree asset or decide how to handle this, we can address this code
            //this.gameObject.SetActive(false);
            //this.deadTree.SetActive(true);
        }
        else if (this._canPickup 
            && this._pickupOkay 
            && Input.GetButton(PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Primary : Constants.Input_P2Primary))
        {
            StartCoroutine(GetCarrot());
        }
    }

    private IEnumerator GetCarrot()
    {
        var text = this._pickup.GetComponent<TextMeshProUGUI>();
        text.text = "";
        this.rabbitAnimator.SetTrigger(Constants.Anim_Tree);
        this._pickupOkay = false;

        Quaternion endQuat = Quaternion.LookRotation(this.transform.position - this.rabbit.transform.position, Vector3.up);
        this.lerpScript.LerpRotation(this.rabbit.transform, this.rabbit.transform.rotation, endQuat, this.rabbitRotateTowardTreeTime);

        Vector3 treePosWithRabbitY = new Vector3(this.transform.position.x, this.rabbit.transform.position.y, this.transform.position.z);
        Vector3 endPos = treePosWithRabbitY 
            + (Vector3.Normalize(this.rabbit.transform.position - treePosWithRabbitY) * this.distanceFromTreeMultiplier);
        this.lerpScript.LerpPosition(this.rabbit.transform, this.rabbit.transform.position, endPos, this.rabbitMoveTowardTreeTime);

        this.rabbitMovement.PauseMovement(this.rabbitShakeTreeTime);

        yield return new WaitForSeconds(this.pickupWait);
        this.rabbitMovement.CanMove = true;

        if (this.numberOfCarrots > 0)
        {
            StartCoroutine(DropBucket());
        }

        this._treeChecked = true;
    }

    private IEnumerator DropBucket()
    {
        // Drop bucket
        this.basketRb.isKinematic = false;
        Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        this.basketRb.AddForce(force * this.basketForce, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        this.basketRb.AddTorque(torque * this.basketForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        while (this.numberOfCarrots > 0)
        {
            CreateCarrot();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the location for carrots to spawn in the basket
        Gizmos.DrawWireSphere(this.transform.TransformPoint(this.basketPoint), 0.1f);
    }
}
