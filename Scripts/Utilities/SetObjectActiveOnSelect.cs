using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetObjectActiveOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private GameObject objectToSetActive;

    private void Awake()
    {
        this.objectToSetActive.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.objectToSetActive.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.objectToSetActive.SetActive(true);
    }
}
