using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    private Level Level;
    void Start()
    {
        Level = FindObjectOfType<Level>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        // TODO instead maybe call a function on the player to "die", playing some animation or effect and then restarting
        // this works for now though
        if (collision.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) {
                player.Die();
            }
        }
    }
}
