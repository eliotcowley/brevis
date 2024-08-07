using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    private float _aspect = 0;

    private float currentTime = 0;

    private Material m_material;
    private bool isFading = false;

    void Start()
    {
        m_material = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Only adjust if aspect ratio changed
        if (_aspect != Camera.main.aspect)
        {
            ResizeOverlay();
        }

        if (isFading)
        {
            var current = m_material.color;
            current.a = Mathf.Lerp(Constants.Full_Opacity, Constants.Full_Transparency, currentTime / Constants.SmokeScreen_Fade);
            currentTime += Time.deltaTime;
            m_material.color = current;

            isFading = currentTime < Constants.SmokeScreen_Fade;
        }
    }

    void ResizeOverlay()
    {
        float quadDepth = this.transform.localPosition.z;

        Vector3 tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, quadDepth));
        Vector3 tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, quadDepth));

        // Get Quad scale
        this._aspect = Camera.main.aspect;
        float widthScale = Mathf.Abs(tl.x - tr.x);
        float heightScale = widthScale / _aspect;

        this.transform.localScale = new Vector3(widthScale, heightScale, this.transform.localScale.z);
    }

    public void EnableSmokescreen()
    {
        if (this.enabled && !isFading)
        {
            var current = m_material.color;
            current.a = Constants.Full_Opacity;
            m_material.color = current;
            currentTime = 0;
            isFading = true;
        }
    }
}
