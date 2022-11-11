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
        Debug.Log(PreviousBestTime);
    }

    void Update() {
        if (!IsLevelEnded) {

            if (Input.GetButtonDown("Restart")) {
                Restart();
            }

            if (TimeRemaining < 0.0) {
                EndLevel(false);
            }

            TimeRemaining -= Time.deltaTime;
        }                
    }

    public void EndLevel(bool isSuccessful) {
        Save(GetCurrentTime());
        IsLevelEnded = true;
        IsLevelSuccessful = isSuccessful;
        Time.timeScale = 0.0f;
    }

    public void Restart() {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name));
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

    IEnumerator LoadSceneAsync(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public void Save(float newTime) {
        String sceneName = SceneManager.GetActiveScene().name;

        float currentBestTime = GetBestTime();
        if (newTime < currentBestTime) {
            PlayerPrefs.SetFloat(sceneName, newTime);
            PlayerPrefs.Save();
            Debug.Log("Saving: " + newTime);
        }                
    }

    public float GetBestTime() {
        String sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.HasKey(sceneName) ? PlayerPrefs.GetFloat(sceneName) : float.MaxValue;        
    }  

}
