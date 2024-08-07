using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeCamera : MonoBehaviour
{
    [SerializeField]
    private Camera P1Cam;
    [SerializeField]
    private Camera P2Cam;
    [SerializeField]
    private Transform rabbit;
    [SerializeField]
    private Transform dog;

    private Vector3 midpoint;
    private bool lastStatusIsActive = false;
    private Camera mergeCamera;

    void Start()
    {
        mergeCamera = this.GetComponent<Camera>();
    }

    void Update()
    {
        // Check distance of characters and toggle cameras
        var isActive = (Vector3.Distance(rabbit.position, dog.position) <= Constants.Camera_Toggle_Distance);
        if (isActive == !lastStatusIsActive) // XOR
        {
            mergeCamera.enabled = isActive;
            P1Cam.enabled = !isActive;
            P2Cam.enabled = !isActive;
            lastStatusIsActive = isActive;
        }
        

        // Average camera position
        this.transform.position = (P1Cam.transform.position + P2Cam.transform.position) / 2;
    }
}
