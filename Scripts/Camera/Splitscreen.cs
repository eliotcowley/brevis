using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitscreen : MonoBehaviour
{
    [SerializeField]
    private Camera otherCam;

    private float _aspect = 0;

    // Update is called once per frame
    void Update()
    {
        // Only adjust if aspect ratio changed
        if (_aspect != Camera.main.aspect)
        {
            ResizeOverlay();
        }
    }

    public void ResizeOverlay()
    {
        float quadDepth = this.transform.localPosition.z;

        Vector3 tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, quadDepth));
        Vector3 tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, quadDepth));

        // Get Quad scale
        this._aspect = Camera.main.aspect;
        float widthScale = Mathf.Abs(tl.x - tr.x);
        float heightScale = widthScale / _aspect;

        this.transform.localScale = new Vector3(widthScale, heightScale, this.transform.localScale.z);

        ResizeTexture();
    }

    private void ResizeTexture()
    {
        // Create the render texture and name it with the dimensions used
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        rt.name = $"Overlay {Screen.width}x{Screen.height}";

        // Link render texture to camera and set in material
        otherCam.targetTexture = rt;
        var mat = this.GetComponent<MeshRenderer>().material;
        // _MainTexture is the value used in the shader
        mat.SetTexture("_MainTexture", rt);
    }
}
