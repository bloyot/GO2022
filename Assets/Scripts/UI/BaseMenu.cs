using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{

    protected CanvasGroup CanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        Toggle(false);
        Init();
    }

    protected virtual void Init() {}
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
