﻿using UnityEngine;
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
            ActualIndex = value;
        }
    }

    public int ActualIndex;

    private bool _calibrated = true;
    private float _dead;
    private int _index;

    public void Update()
    {
        if (!_calibrated)
        {
            var x = Input.GetAxis("Joystick_" + ActualIndex + "_Left_x");
            var y = Input.GetAxis("Joystick_" + ActualIndex + "_Left_y");
            var d = Mathf.Sqrt(x * x + y * y);
            if (d < 0.4f && d != 0)
            {
                _dead = d * 2f;
                _calibrated = true;
            }
        }

        // Walk
        var input = Input.GetAxis("Joystick_" + ActualIndex + "_Left_x");
        if (Mathf.Abs(input) < _dead) input = 0;
        Player.Walk(input);

        // Jump
        if (Input.GetKeyDown("joystick " + ActualIndex + " button 0")) Player.Jump();
        if (Input.GetKeyUp("joystick " + ActualIndex + " button 0")) Player.StopJump();

        // Shoot
        if (Input.GetAxis("Joystick_" + ActualIndex + "_Trigger") < 0) Core.UseItems();
    }
}
