using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : BaseLevelMenu
{
    public TextMeshProUGUI CurrentTime;
    public TextMeshProUGUI Offset;
    public TextMeshProUGUI BestTime;

    public Color PositiveOffsetColor;
    public Color NegativeOffsetColor;
    public Color NeutralOffsetColor;

    public GameObject NextLevelButton;

    protected override void Init() {
        base.Init();
        if (SceneManager.GetActiveScene().name == "Level3") {
            NextLevelButton.SetActive(false);
        }
    }

    public void SetTimes(float currentTime, float previousBestTime) {
        float bestTime = previousBestTime < currentTime ? previousBestTime : currentTime;
        float offsetTime = currentTime - previousBestTime;
        CurrentTime.text = currentTime.ToString("F2");
        BestTime.text = bestTime.ToString("F2");

        string offsetSymbol = "";
        if (offsetTime > 0) {
            Offset.color = NegativeOffsetColor;
            offsetSymbol = "+";
        } else if (offsetTime < 0) {
            Offset.color = PositiveOffsetColor;
            offsetSymbol = "-";
        } else {
            Offset.color = NeutralOffsetColor;
        }

        // check for first time 
        Offset.text = offsetTime < -1000.0f ? "" : "(" + offsetSymbol + Mathf.Abs(offsetTime).ToString("F2") + ")";        
    }

    public void NextLevel() {
        // hacky but good enough for our purposes since we only have 3 levels
        if (SceneManager.GetActiveScene().name == "Level1") {
            StartCoroutine(Scenes.LoadSceneAsync("Level2"));
        }

        if (SceneManager.GetActiveScene().name == "Level2") {
            StartCoroutine(Scenes.LoadSceneAsync("Level3"));
        }

    }

}
