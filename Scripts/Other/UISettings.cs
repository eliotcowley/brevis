using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISettings : MonoBehaviour
{
    [SerializeField]
    private GameObject firstSelectedGameObject;

    private GameObject lastSelectedGameObject;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (Application.isEditor)
        {
            FpsCounter.Instance.Dummy();
        }
    }

    private void Update()
    {
        if (EventSystem.current == null)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (this.lastSelectedGameObject == null)
            {
                this.lastSelectedGameObject = this.firstSelectedGameObject;
            }

            EventSystem.current.SetSelectedGameObject(this.lastSelectedGameObject);
        }
        else
        {
            if (this.lastSelectedGameObject != EventSystem.current.currentSelectedGameObject && this.lastSelectedGameObject != null)
            {
                SFXPlayer.Instance.PlaySelectSound();
            }
            this.lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }

        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
    }
}
