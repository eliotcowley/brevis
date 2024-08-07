using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public float RotateTowardsCarrotTime = 0.2f;

    [HideInInspector]
    public bool Dead = false;

    [SerializeField]
    private GameObject holePrefab;

    [SerializeField]
    private GameObject holeTempPrefab;

    [SerializeField]
    private Material holeTempObstructedMaterial;

    [SerializeField]
    private int maxHoles = 2;

    [SerializeField]
    private float holeDistanceMultiplier = 0.7f;

    [SerializeField]
    private float holeTime = 5f;

    [SerializeField]
    private SmokeScreen P1Smoke;

    [SerializeField]
    private SmokeScreen P2Smoke;

    [SerializeField]
    private TextMeshProUGUI holeTimerText;

    private bool isPlayer1 = true;
    private readonly List<Transform> holes = new List<Transform>();
    private Transform holeTemp;
    private MeshRenderer holeTempMeshRenderer;
    private SphereCollider holeTempSphereCollider;
    private float holeTimer = 0f;
    private bool isHoleTimerOn = false;
    private string inputSecondary;
    private bool smokeScreenReady = true;
    private Vector3 newHolePosition;
    private bool isHoleObstructed = false;
    private Material holeTempNormalMaterial;

    private List<Transform> ActiveHoles 
    {
        get => (from hole in this.holes
                where hole.gameObject.activeSelf
                select hole).ToList();
    }

    private void Start()
    {
        GameManager.Instance.RabbitStartingPosition = this.transform.position;
        this.isPlayer1 = PersistentData.P1Character == Character.Rabbit;
        this.inputSecondary = this.isPlayer1 ? Constants.Input_P1Secondary : Constants.Input_P2Secondary;
        this.holeTimerText.SetText("");
        this.holeTemp = Instantiate(this.holeTempPrefab).transform;
        this.holeTempMeshRenderer = this.holeTemp.GetComponentInChildren<MeshRenderer>();
        this.holeTempSphereCollider = this.holeTemp.GetComponent<SphereCollider>();
        this.holeTempNormalMaterial = this.holeTempMeshRenderer.material;
        this.holeTemp.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.Dead)
        {
            return;
        }

        // Since we're removing secondary and tertiary attacks, I'm commenting this out. Can't bring myself to delete it.
        //if (!RpsManager.Instance.InBulletTime)
        //{
        //    if (Input.GetButtonDown(this.inputSecondary))
        //    {
        //        if (this.ActiveHoles.Count < this.maxHoles)
        //        {
        //            StartHoleTimer();
        //        }
        //    }

        //    if (this.smokeScreenReady && Vector3.Distance(this.P1Smoke.transform.position, this.P2Smoke.transform.position) < Constants.SmokeScreen_Distance)
        //    {
        //        if (this.isPlayer1 && Input.GetButtonDown(Constants.Input_P1Special))
        //        {
        //            this.P2Smoke.EnableSmokescreen();
        //            StartCoroutine(SmokescreenCooldown());
        //        }
        //        else if (!this.isPlayer1 && Input.GetButtonDown(Constants.Input_P2Special))
        //        {
        //            this.P1Smoke.EnableSmokescreen();
        //            StartCoroutine(SmokescreenCooldown());
        //        }
        //    }
        //}

        //if (this.isHoleTimerOn)
        //{
        //    this.holeTimerText.SetText(Mathf.Ceil(this.holeTime - this.holeTimer).ToString());
        //    this.newHolePosition = this.transform.position + (this.transform.forward * this.holeDistanceMultiplier);
        //    this.holeTemp.position = this.newHolePosition;
        //    bool holeCheck = CheckHoleObstructed();

        //    if (this.isHoleObstructed != holeCheck)
        //    {
        //        this.isHoleObstructed = holeCheck;
        //        this.holeTempMeshRenderer.material = this.isHoleObstructed ? this.holeTempObstructedMaterial : this.holeTempNormalMaterial;
        //    }

        //    if (Input.GetButton(this.inputSecondary))
        //    {
        //        this.holeTimer += Time.deltaTime;

        //        if (this.holeTimer >= this.holeTime)
        //        {
        //            CreateHole();
        //        }
        //    }
        //    else
        //    {
        //        ResetHoleTimer();
        //    }
        //}
        //else
        //{
        //    this.holeTimerText.SetText("");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Hole) && this.ActiveHoles.Count == this.maxHoles)
        {
            this.transform.position = (from hole in this.holes
                                       where hole != other.transform
                                       select hole).First().position;

            foreach (Transform hole in this.holes)
            {
                hole.gameObject.SetActive(false);
            }
        }
    }

    private void StartHoleTimer()
    {
        this.holeTimer = 0f;
        this.isHoleTimerOn = true;
        this.holeTemp.gameObject.SetActive(true);
    }

    private void CreateHole()
    {
        Transform newHole = this.holes.FirstOrDefault(hole => !hole.gameObject.activeSelf);

        if (newHole == null)
        {
            newHole = Instantiate(this.holePrefab).transform;
            newHole.gameObject.SetActive(false);
            this.holes.Add(newHole);
        }

        if (!this.isHoleObstructed)
        {
            newHole.position = this.newHolePosition;
            newHole.gameObject.SetActive(true);
            this.holeTimerText.SetText("");
        }
        else
        {
            this.holeTimerText.SetText("X");
        }

        this.isHoleTimerOn = false;
        this.holeTemp.gameObject.SetActive(false);
    }

    public void ResetHoleTimer()
    {
        this.isHoleTimerOn = false;
        this.holeTimerText.SetText("");
        this.holeTemp.gameObject.SetActive(false);
    }

    private bool CheckHoleObstructed()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.newHolePosition, this.holeTempSphereCollider.radius);

        foreach (Collider collider in hitColliders)
        {
            if (!collider.CompareTag(Constants.Tag_Ground) 
                && !collider.CompareTag(Constants.Tag_Hole)
                && !collider.CompareTag(Constants.Tag_BulletTimeSensor))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator SmokescreenCooldown()
    {
        this.smokeScreenReady = false;
        yield return new WaitForSeconds(Constants.SmokeScreen_Cooldown);
        this.smokeScreenReady = true;
    }
}
