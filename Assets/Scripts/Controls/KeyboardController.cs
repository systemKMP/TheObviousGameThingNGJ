using UnityEngine;
using System.Collections;

public class KeyboardController : PlayerController
{
    public string WalkInputAxis;
    public KeyCode JumpInput;
    public KeyCode ShootInput = KeyCode.T;

    public void Update()
    {
        if(Player == null) { Debug.LogWarning("You haven't attached a player to me, moron"); return; }

        Player.Walk(Input.GetAxis(WalkInputAxis));

        if (Input.GetKeyDown(JumpInput)) Player.Jump();
        if (Input.GetKeyUp(JumpInput)) Player.StopJump();

        if (Input.GetKey(ShootInput)) Core.UseItems();
    }
}
