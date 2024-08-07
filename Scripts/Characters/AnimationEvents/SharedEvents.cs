using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stepSounds;
    [SerializeField]
    private AudioClip winSound;

    private void Step(AnimationEvent evt)
    {
        // If weight of clip is above a certain threshold, play step sound
        if (evt.animatorClipInfo.weight > Constants.Sound_Threshold)
        {
            GameManager.Instance.PlaySound(SelectRandomSound(stepSounds), 0.65f);
        }
    }

    protected virtual void Win(AnimationEvent evt)
    {
        GameManager.Instance.PlaySound(winSound);
    }

    private AudioClip SelectRandomSound(AudioClip[] sounds)
    {
        var track = Random.Range(0, sounds.Length);

        return sounds[track];
    }
}
