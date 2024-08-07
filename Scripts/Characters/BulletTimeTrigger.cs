using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletTimeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_BulletTimeSensor) 
            && !RpsManager.Instance.InBulletTime
            && !GameManager.Instance.GameOver)
        {
            RpsManager.Instance.StartBulletTime();
        }
    }
}
