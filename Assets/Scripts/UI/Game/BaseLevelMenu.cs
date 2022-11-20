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
        StartCoroutine(Scenes.LoadSceneAsync("MainMenu"));       
    }
}
