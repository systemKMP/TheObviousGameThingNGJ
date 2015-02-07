using UnityEngine;
using System.Collections;

public class ControllerController : MonoBehaviour
{
    public PlayerMovement Player;
    [Range(1, 8)]
    public int Controller = 1;

    private float _presign = 0;

    PlayerCore core;

    void Start()
    {
        core = gameObject.GetComponent<PlayerCore>();
    }

    public void Update()
    {
        // Walk
        Player.Walk(Input.GetAxis("Joystick_" + Controller + "_Left_x"));

        // Jump
        if (Input.GetKeyDown("joystick " + Controller + " button 0")) Player.Jump();

        // Shoot
        if (Input.GetAxis("Joystick_" + Controller + "_Trigger") < 0)
        {
            core.UseItems();
        }
    }
}
