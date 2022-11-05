using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIController : MonoBehaviour
{

    public PlayerController Player;

    public TextMeshProUGUI OnWall;
    public TextMeshProUGUI WallSide;
    public TextMeshProUGUI OnGround;
    public TextMeshProUGUI InputDir;
    public TextMeshProUGUI InputJump;
    public TextMeshProUGUI InputGrab;

    // Update is called once per frame
    void Update()
    {
        OnWall.text = "On Wall: " + Player.OnWall;
        WallSide.text = "Wall Side: " + Player.WallSide;
        OnGround.text = "On Ground: " + Player.OnGround;
        InputDir.text = "Input Direction: " + Player.InputDir;
        InputJump.text = "Input Jump: " + Player.InputJumpHeld;
        InputGrab.text = "Input Grab: " + Player.InputWallGrab;
    }
}
