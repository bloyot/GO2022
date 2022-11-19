using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    // params
    public Rect LevelBounds;
    public float LevelTime;

    // public
    public float TimeRemaining { get; set; }
    public int TotalScrews { get; set; }
    public int CollectedScrews { get; set; } = 0;

    public bool IsLevelEnded { get; set; } = false;
    public bool IsLevelSuccessful { get; set; } = false;
    private List<Screw> Screws { get; set; }
    public float PreviousBestTime { get; set; }

    void Start()
    {
        Screws = new List<Screw>(FindObjectsOfType<Screw>());
        TotalScrews = Screws.Count;
        TimeRemaining = LevelTime;
        PreviousBestTime = GetBestTime();        
    }

    void Update() {
        if (!IsLevelEnded) {

            if (Input.GetButtonDown("Restart")) {
                Restart();
            }

            if (TimeRemaining <= 0.0) {
                EndLevel(false);
            }
            
            // time scale accounts for pause
            TimeRemaining = Mathf.Clamp(TimeRemaining - (Time.deltaTime * Time.timeScale), 0.0f, LevelTime);
        }                
    }

    public void EndLevel(bool isSuccessful) {
        if (isSuccessful) {
            Save(GetCurrentTime());
        }

        IsLevelEnded = true;
        IsLevelSuccessful = isSuccessful;
        Time.timeScale = 0.0f;
    }

    public void Restart() {
        StartCoroutine(Scenes.LoadSceneAsync(SceneManager.GetActiveScene().name));
    }
    public void CollectScrew() {
        CollectedScrews++;
    }

    public bool AllScrewsCollected() {
        return TotalScrews == CollectedScrews;
    }

    // return the players current time on the level
    public float GetCurrentTime() {
        return LevelTime - TimeRemaining;
    }

    public void Save(float newTime) {
        String sceneName = SceneManager.GetActiveScene().name;

        float currentBestTime = GetBestTime();
        if (newTime < currentBestTime) {
            PlayerPrefs.SetFloat(sceneName, newTime);
            PlayerPrefs.Save();            
        }                
    }

    public float GetBestTime() {
        String sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.HasKey(sceneName) ? PlayerPrefs.GetFloat(sceneName) : float.MaxValue;        
    }  

}
