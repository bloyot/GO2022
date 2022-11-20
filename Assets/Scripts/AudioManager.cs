using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource TitleMusic;
    public AudioSource LevelMusic;

    void Awake()
    {
        // a bit hacky but since restarting a scene results in a second instance of this, just destroy this if one already exists
        // TODO probably the correct way to do this is a singleton pattern for the audio manager across scenes
        if (FindObjectsOfType<AudioManager>().Length > 1) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            if (LevelMusic.isPlaying) {
                LevelMusic.Stop();
            }
            if (!TitleMusic.isPlaying) {
                TitleMusic.Play();
            }
        }

        if (SceneManager.GetActiveScene().name != "MainMenu") {
            if (TitleMusic.isPlaying) {
                TitleMusic.Stop();
            }
            if (!LevelMusic.isPlaying) {
                LevelMusic.Play();
            }
        }
    }
}
