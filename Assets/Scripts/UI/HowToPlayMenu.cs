using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenu : BaseMenu
{
    public BaseMenu PreviousMenu;
    public Button BackButton;

    protected override void Init() {
        base.Init();
        BackButton.onClick.AddListener(() => Back());
    }
    public void Back() {
        Toggle(false);
        PreviousMenu.Toggle(true);
    }

}
