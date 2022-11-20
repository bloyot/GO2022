using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BaseLevelMenu
{

    public bool Paused { get; set; } = false;    

    public override void Toggle(bool active) {        
        base.Toggle(active);
        Paused = active;
        Time.timeScale = Paused ? 0.0f : 1.0f;
    }

    public void Controls() {
        Debug.Log("Controls not yet implemented");
        // TODO show controls menu screen        
    }

    public void Resume() {
        Toggle(false);
    }

}