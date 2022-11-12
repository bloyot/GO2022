using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define base functions for any menus shown during the level
[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseLevelMenu : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;
    void Start() {
        CanvasGroup = GetComponent<CanvasGroup>();
        Toggle(false);
    }    

    public virtual void Toggle(bool active) {        
        if (active) {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
            Cursor.visible = true;
        }
        else {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            Cursor.visible = false;
        }
    }
}
