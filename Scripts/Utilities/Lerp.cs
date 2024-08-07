using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Not a very generalized implementation. I'd like to do something more sophisticated so you don't have to define different variables for each use case,
// but probably not worth it at this point.
public class Lerp : MonoBehaviour
{
    private Quaternion startQuat;
    private Quaternion endQuat;

    private Vector3 startPos;
    private Vector3 endPos;

    private float rotTimer = 0f;
    private float rotLerpTime = 1f;

    private float posTimer = 0f;
    private float posLerpTime = 1f;

    private bool isRotLerping = false;
    private bool isPosLerping = false;

    private Transform lerpTransform;

    public void LerpRotation(Transform lerpTransform, Quaternion startQuat, Quaternion endQuat, float lerpTime)
    {
        this.isRotLerping = true;
        this.startQuat = startQuat;
        this.endQuat = endQuat;
        this.rotLerpTime = lerpTime;
        this.lerpTransform = lerpTransform;
    }

    public void LerpRotateTowardsObject(Transform lerpTransform, Transform target, float lerpTime)
    {
        this.isRotLerping = true;
        this.rotLerpTime = lerpTime;
        this.lerpTransform = lerpTransform;
        this.startQuat = lerpTransform.rotation;

        Vector3 lookVector = Quaternion.LookRotation(target.position - lerpTransform.position, Vector3.up).eulerAngles;
        Vector3 startVector = lerpTransform.rotation.eulerAngles;
        Vector3 rotateOnlyY = new Vector3(startVector.x, lookVector.y, startVector.z);
        this.endQuat = Quaternion.Euler(rotateOnlyY);
    }

    public void LerpPosition(Transform lerpTransform, Vector3 startPos, Vector3 endPos, float lerpTime)
    {
        this.isPosLerping = true;
        this.startPos = startPos;
        this.endPos = endPos;
        this.posLerpTime = lerpTime;
        this.lerpTransform = lerpTransform;
    }

    private void Update()
    {
        if (this.rotTimer >= this.rotLerpTime)
        {
            this.isRotLerping = false;
            this.rotTimer = 0f;
        }

        if (this.isRotLerping)
        {
            this.rotTimer += Time.deltaTime;
            this.lerpTransform.rotation = Quaternion.Lerp(this.startQuat, this.endQuat, this.rotTimer / this.rotLerpTime);
        }

        if (this.posTimer >= this.posLerpTime)
        {
            this.isPosLerping = false;
            this.posTimer = 0f;
        }

        if (this.isPosLerping)
        {
            this.posTimer += Time.deltaTime;
            this.lerpTransform.position = Vector3.Lerp(this.startPos, this.endPos, this.posTimer / this.posLerpTime);
        }
    }
}
