using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Vector2 Offset;

    private GameObject player;
    private Level level;

    void Start() {
        player = GameObject.FindWithTag("Player");        
        level = FindObjectOfType<Level>();    
    }

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(player.transform.position.x + Offset.x, level.LevelBounds.x - level.LevelBounds.width / 2, level.LevelBounds.x + level.LevelBounds.width / 2);
        float posY = Mathf.Clamp(player.transform.position.y + Offset.y, level.LevelBounds.y - level.LevelBounds.height / 2, level.LevelBounds.y + level.LevelBounds.width / 2);
        transform.position = new Vector3(posX, posY, -10);
    }
}
