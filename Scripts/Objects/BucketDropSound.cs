using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BucketDropSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tag_Ground))
        {
            this.audioSource.Play();
        }
    }
}
