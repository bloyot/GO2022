using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : BaseMenu
{

    public LevelSelectMenu LevelSelectMenu;
    public HowToPlayMenu HowToPlayMenu;
    public HowToPlayMenu CreditsMenu;

    protected override void Init() {       
        base.Init();
        Toggle(true);
    }

    public void Levels() {
        Toggle(false);
        LevelSelectMenu.Toggle(true);
    }

    public void HowToPlay() {
        Toggle(false);
        HowToPlayMenu.Toggle(true);
    }

    public void Credits() {
        Toggle(false);
        CreditsMenu.Toggle(true);
    }

    public void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();        
    }
}
