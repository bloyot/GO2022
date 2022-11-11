using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public Level Level;

    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Screws;
    public TextMeshProUGUI Return;

    public PauseMenu PauseMenu;
    public LevelCompleteMenu LevelCompleteMenu;

    private bool paused;    

    void Start() {
        Level = FindObjectOfType<Level>();
        Return.enabled = false;
        PauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleScrews();
        HandlePause();
        HandleEndLevel(); 
    }

    private void HandleEndLevel() {
        if (Level.IsLevelEnded && Level.IsLevelSuccessful) {
            float currentTime = Level.GetCurrentTime();            
            LevelCompleteMenu.Toggle(true);
            LevelCompleteMenu.SetTimes(currentTime, Level.PreviousBestTime);                        
        } else if (Level.IsLevelEnded) {
            // TODO 
        }
    }

    private void HandleScrews() {
        Timer.text = Level.TimeRemaining.ToString("F2") + "s";
        Screws.text = "Collected " + Level.CollectedScrews + " of " + Level.TotalScrews;

        if (Level.CollectedScrews == Level.TotalScrews) {
            Return.enabled = true;
        }
    }

    private void HandlePause() {
        if (Input.GetButtonDown("Pause") && !Level.IsLevelEnded) {
            if (paused) {
                paused = false;
                PauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else {
                paused = true;
                PauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }
}
