using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : BaseMenu
{

    public MainMenu MainMenu;
    
    private List<Button> LevelButtons;

    protected override void Init() {
        base.Init();
        LevelButtons = new List<Button>(GetComponentsInChildren<Button>());
        foreach (Button button in LevelButtons) {
            if (button.tag == "LevelSelect") {
                button.onClick.AddListener(() => SelectLevel(button.name));
            }
        }
    }

    public void Back() {
        Toggle(false);
        MainMenu.Toggle(true);
    }

    public void SelectLevel(string levelName) {
        StartCoroutine(Scenes.LoadSceneAsync(levelName));
    }

}
