using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEvents : SharedEvents
{
    [SerializeField]
    private AudioClip sniffSound;

    protected override void Win(AnimationEvent evt)
    {
        base.Win(evt);
        GameManager.Instance.DogScript.DropCarrotsOrWin();
        StartCoroutine(Howl());
    }

    private void Sniff(AnimationEvent evt)
    {
        GameManager.Instance.PlaySound(sniffSound, 0.7f);
    }

    private IEnumerator Howl()
    {
        yield return new WaitForSeconds(Constants.Dog_Win);

        var dogAnimator = this.GetComponent<Animator>();
        dogAnimator.Play(Constants.Anim_Win);
    }
}
