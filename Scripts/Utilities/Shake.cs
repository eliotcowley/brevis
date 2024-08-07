using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField]
    private float shakeRange = 20f;

    [SerializeField]
    private float shakeTime = 0.5f;

    private bool isShaking = false;
    private float timer = 0f;
    private Quaternion origRotation;

    public void Shake2D()
    {
        if (!this.isShaking)
        {
            this.isShaking = true;
        }
    }

    private void Start()
    {
        this.origRotation = this.transform.rotation;
    }

    private void Update()
    {
        if (this.isShaking)
        {
            float shakeZ = Random.value * this.shakeRange - (this.shakeRange / 2);
            this.transform.eulerAngles = new Vector3(this.origRotation.x, this.origRotation.y, this.origRotation.z + shakeZ);
            this.timer += Time.deltaTime;
        }

        if (this.timer >= this.shakeTime)
        {
            this.isShaking = false;
            this.timer = 0f;
            this.transform.rotation = this.origRotation;
        }
    }
}
