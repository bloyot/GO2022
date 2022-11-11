using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{    
    // the sprites to show the level end state
    public GameObject InactiveSprite;
    public GameObject ActiveSprite;

    private Level level;
    
    void Start() {
        level = FindObjectOfType<Level>();
        InactiveSprite.SetActive(true);
        ActiveSprite.SetActive(false);
    }

    void Update() {
        if (level.AllScrewsCollected()) {
            InactiveSprite.SetActive(false);
            ActiveSprite.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && level.AllScrewsCollected()) {
            level.EndLevel(true);                    
        }
    }
}
