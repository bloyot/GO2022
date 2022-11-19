using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompleteMenu : BaseLevelMenu
{
    public TextMeshProUGUI CurrentTime;
    public TextMeshProUGUI Offset;
    public TextMeshProUGUI BestTime;

    public Color PositiveOffsetColor;
    public Color NegativeOffsetColor;
    public Color NeutralOffsetColor;

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
        Offset.text = offsetTime > 1000.0f ? "" : "(" + offsetSymbol + Mathf.Abs(offsetTime).ToString("F2") + ")";        
    }

    public void NextLevel() {
        Debug.Log("next level not implemented yet");
        // TODO load next level
    }

}
