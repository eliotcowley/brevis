using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitEvents : SharedEvents
{
    [SerializeField]
    private AudioClip depositSound;
    [SerializeField]
    private AudioClip pickUpSound;

    protected override void Win(AnimationEvent evt)
    {
        base.Win(evt);
        StartCoroutine(GameManager.Instance.DogScript.Stun());
    }

    private void Deposit(AnimationEvent evt)
    {
        // If weight of clip is above a certain threshold, play step sound
        if (evt.animatorClipInfo.weight > Constants.Sound_Threshold)
        {
            GameManager.Instance.PlaySound(depositSound, 0.75f);
        }
    }

    private void PickUp(AnimationEvent evt)
    {
        // If weight of clip is above a certain threshold, play step sound
        if (evt.animatorClipInfo.weight > Constants.Sound_Threshold)
        {
            GameManager.Instance.PlaySound(pickUpSound);
        }
        GameManager.Instance.PickUpActive = false;
    }

    private void Die(AnimationEvent evt)
    {
        GameManager.Instance.TakeLife();
    }
}
