﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Movement Mover;

    public AnimationCurve WalkCurve;
    public float MaxWalkSpeed;
    public float WalkFloatiness;
    public AnimationCurve AirWalkCurve;
    public float AirMaxWalkSpeed;
    public float AirWalkFloatiness;
    public float JumpForce;
    public int MaxJumps;

    private float _direction;
    private float _prewalk;
    private float _walk;
    private float _time;
    private float _jump;
    private int _jumps;
    private bool _stopped;
    private readonly List<Transform> _grounds = new List<Transform>(); 

    public void Update()
    {
        if (Mover == null) { Debug.LogWarning("You haven't attached a movement script to me, moron"); return; }

        if (!_stopped && _direction == 0)
        {
            Mover.rigidbody2D.AddForce(new Vector2(-Mover.rigidbody2D.velocity.x, 0) * Mover.rigidbody2D.mass, ForceMode2D.Impulse);
            _stopped = true;
        }

        var curve = (IsGrounded() ? WalkCurve : AirWalkCurve);
        var floatiness = (IsGrounded() ? WalkFloatiness : AirWalkFloatiness);
        var maxspeed = (IsGrounded() ? MaxWalkSpeed : AirMaxWalkSpeed);

        float desiredWalk = 0;
        if (_direction != 0)
        {
            desiredWalk = Mathf.Clamp(_direction, -maxspeed, maxspeed) * maxspeed;
        }
        if (_time < floatiness)
        {
            _walk = Mathf.Lerp(_prewalk, desiredWalk, curve.Evaluate(_time / floatiness));
            _time += Time.deltaTime;
        }
        else
        {
            _walk = desiredWalk;
        }

        Mover.Move = new Vector2(_walk, _jump);
        _jump = 0;
    }

    public void Walk(float direction)
    {
        _stopped = false;
        if (_direction != direction) { _time = 0; _prewalk = _walk; }
        _direction = direction;

        if (direction > 0.02f)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (direction < -0.02f)
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void Jump()
    {
        if (!CanJump()) return;
        _jumps--;
        _jump += JumpForce;
    }

    public bool CanJump()
    {
        return _jumps > 0;
    }

    public bool IsGrounded()
    {
        return _grounds.Count > 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Any(contact => Vector2.Dot(contact.normal, Vector2.up) > 0.6f))
        {
            _jumps = MaxJumps;
            _grounds.Add(collision.transform);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (_grounds.Contains(collision.transform)) _grounds.Remove(collision.transform);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Any(contact => Mathf.Abs(Vector2.Dot(contact.normal, Vector2.right)) > 0.8f))
        {
            _direction = 0;
        }
    }
}
