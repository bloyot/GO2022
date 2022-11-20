using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : BaseMenu
{

    public LevelSelectMenu LevelSelectMenu;

    protected override void Init() {       
        base.Init();
        Toggle(true);
    }

    public void Levels() {
        Toggle(false);
        LevelSelectMenu.Toggle(true);
    }

    public void Controls() {
        Debug.Log("Controls not yet implemented");
        // TODO show controls menu screen        
    }

    public void Credits() {
        Debug.Log("Credits not yet implemented");
        // TODO show controls menu screen        
    }

    public void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();        
    }
}
