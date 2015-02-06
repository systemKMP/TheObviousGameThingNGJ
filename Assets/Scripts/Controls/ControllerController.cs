using UnityEngine;
using System.Collections;

public class ControllerController : MonoBehaviour
{
    public PlayerMovement Player;
    [Range(1,8)]
    public int Controller = 1;

    public void Update()
    {
        Player.Walk(Input.GetAxis("Joystick_"+Controller+"_Left_x"));
        if (Input.GetKey("joystick " + (Controller) + " button 0")) Player.Jump();
    }
}
