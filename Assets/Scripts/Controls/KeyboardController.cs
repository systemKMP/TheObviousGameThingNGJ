using UnityEngine;
using System.Collections;

public class KeyboardController : PlayerController
{
    public KeyCode JumpInput = KeyCode.W;
    public KeyCode RightInput = KeyCode.D;
    public KeyCode LeftInput = KeyCode.A;
    public KeyCode ShootInput = KeyCode.G;

    public void Update()
    {
        if(Player == null) { Debug.LogWarning("You haven't attached a player to me, moron"); return; }

        Player.Walk((Input.GetKey(RightInput) ? 1 : 0) - (Input.GetKey(LeftInput) ? 1 : 0));

        if (Input.GetKeyDown(JumpInput)) Player.Jump();
        if (Input.GetKeyUp(JumpInput)) Player.StopJump();

        if (Input.GetKey(ShootInput)) Core.UseItems();
    }
}
