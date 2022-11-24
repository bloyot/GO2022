using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseLevelMenu
{
    public HowToPlayMenu HowToPlayMenu;

    public bool Paused { get; set; } = false;    

    public override void Toggle(bool active) {        
        base.Toggle(active);
        Paused = active;
        Time.timeScale = Paused ? 0.0f : 1.0f;
    }

    public void HowToPlay() {
        Toggle(false);
        HowToPlayMenu.Toggle(true);
        // TODO show controls menu screen        
    }

    public void Resume() {
        Toggle(false);
    }

}