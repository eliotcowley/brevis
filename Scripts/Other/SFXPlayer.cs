using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Prefab]
public class SFXPlayer : Singleton<SFXPlayer>
{
    public float soundVolume = 1f;

    [SerializeField]
    private AudioClip confirmSound;

    [SerializeField]
    private AudioClip selectSound;

    public void PlayConfirmSound()
    {
        AudioSource.PlayClipAtPoint(confirmSound, Camera.current.transform.position, soundVolume);
    }

    public void PlaySelectSound()
    {
        AudioSource.PlayClipAtPoint(selectSound, Camera.current.transform.position, soundVolume);
    }
}
