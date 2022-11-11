using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LevelCompleteMenu : MonoBehaviour
{
    public TextMeshProUGUI CurrentTime;
    public TextMeshProUGUI Offset;
    public TextMeshProUGUI BestTime;

    public Color PositiveOffsetColor;
    public Color NegativeOffsetColor;
    public Color NeutralOffsetColor;

    public CanvasGroup CanvasGroup;

    void Start() {
        CanvasGroup = GetComponent<CanvasGroup>();
        Toggle(false);
    }

    public void Toggle(bool active) {
        if (active) {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        } else {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;            
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

        Offset.text = "(" + offsetSymbol + Mathf.Abs(offsetTime).ToString("F2") + ")";

    }

}
