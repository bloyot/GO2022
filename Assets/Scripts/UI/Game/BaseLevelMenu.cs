using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define base functions for any menus shown during the level
[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseLevelMenu : BaseMenu
{
    protected Level Level;
    protected override void Init() {
        base.Init();
        Level = FindObjectOfType<Level>();
    }      

    public void Restart() {
        Level.Restart();
    }  

    public void MainMenu() {
        Debug.Log("MainMenu not yet implemented");
        // TODO show controls menu screen        
    }
}
