using UnityEngine;
using System.Collections;

public class JoystickController : PlayerController
{
    public override int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            _calibrated = false;
        }
    }

    private bool _calibrated = true;
    private float _dead;
    private int _index;

    public void Update()
    {
        if (!_calibrated)
        {
            var x = Input.GetAxis("Joystick_" + Index + "_Left_x");
            var y = Input.GetAxis("Joystick_" + Index + "_Left_y");
            var d = Mathf.Sqrt(x * x + y * y);
            if (d < 0.5f && d != 0)
            {
                _dead = d * 3f;
                _calibrated = true;
            }
        }

        // Walk
        var input = Input.GetAxis("Joystick_" + Index + "_Left_x");
        if (Mathf.Abs(input) < _dead) input = 0;
        Player.Walk(input);

        // Jump
        if (Input.GetKeyDown("joystick " + Index + " button 0")) Player.Jump();

        // Shoot
        if (Input.GetAxis("Joystick_" + Index + "_Trigger") < 0) Core.UseItems();
    }
}
