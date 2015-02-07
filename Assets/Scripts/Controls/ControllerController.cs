using UnityEngine;
using System.Collections;

public class ControllerController : MonoBehaviour
{
    public PlayerMovement Player;

    public int Controller
    {
        get { return _controller; }
        set
        {
            _controller = value;
            _calibrated = false;
        }
    }

    private bool _calibrated = true;
    private float _dead;
    private PlayerCore _core;
    private int _controller;
    
    public void Start()
    {
        _core = gameObject.GetComponent<PlayerCore>();
    }

    public void Update()
    {
        if (!_calibrated)
        {
            var x = Input.GetAxis("Joystick_" + Controller + "_Left_x");
            var y = Input.GetAxis("Joystick_" + Controller + "_Left_y");
            var d = Mathf.Sqrt(x * x + y * y);
            if (d < 0.5f)
            {
                _dead = d * 1.5f;
                _calibrated = true;
            }
        }

        // Walk
        var input = Input.GetAxis("Joystick_" + Controller + "_Left_x");
        if (Mathf.Abs(input) < _dead) input = 0;
        Player.Walk(input);

        // Jump
        if (Input.GetKeyDown("joystick " + Controller + " button 0")) Player.Jump();

        // Shoot
        if (Input.GetAxis("Joystick_" + Controller + "_Trigger") < 0) _core.UseItems();
    }
}
