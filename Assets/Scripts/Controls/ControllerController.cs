using UnityEngine;
using System.Collections;

public class ControllerController : MonoBehaviour
{
    public PlayerMovement Player;
    [Range(1, 8)]
    public int Controller = 1;

    private float _presign = 0;

    public void Update()
    {
        // Walk
        Player.Walk(Input.GetAxis("Joystick_" + Controller + "_Left_x"));

        // Jump
        if (Input.GetKey("joystick " + Controller + " button 0")) Player.Jump();

        // Shoot
        var sign = Mathf.Sign(Input.GetAxis("Joystick_" + Controller + "_Trigger"));
        if (sign != _presign && sign == -1) Debug.Log("BANG!"); // TODO shoot
        _presign = sign;
    }
}
