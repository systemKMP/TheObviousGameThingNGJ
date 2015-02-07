using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Movement Mover;

    public AudioSource Audio;

    public AnimationCurve WalkCurve;
    public float MaxWalkSpeed;
    public float WalkFloatiness;
    public AnimationCurve AirWalkCurve;
    public float AirMaxWalkSpeed;
    public float AirWalkFloatiness;
    public float JumpForce;
    public int MaxJumps;
    public float StepTime = 0.2f;

    public AudioClip[] StepClips = new AudioClip[0];

    private float _stepTime;
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

        if (IsGrounded()) _jumps = MaxJumps;
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

        if (_stepTime > 0 && Mathf.Abs(_walk) > 0.01f && IsGrounded())
        {
            _stepTime -= Time.deltaTime;
        }
        else if (Mathf.Abs(_walk) > 0.01f && IsGrounded())
        {
            Audio.clip = StepClips[Random.Range(0, StepClips.Length)];
            Audio.Play();
            _stepTime = StepTime;
        }
        else
        {
            Audio.Stop();
        }

        Mover.Move = new Vector2(_walk, 0);
        if(_jump != 0)rigidbody2D.velocity = Vector2.up*_jump + Vector2.right*rigidbody2D.velocity.x;
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
        var hit = new[]
        {
            Physics2D.Raycast(transform.position+Vector3.right*0.5f-Vector3.up, -Vector2.up, float.PositiveInfinity,
                1 << LayerMask.NameToLayer("Terrain")),
            Physics2D.Raycast(transform.position-Vector3.up, -Vector2.up, float.PositiveInfinity,
                1 << LayerMask.NameToLayer("Terrain")),
            Physics2D.Raycast(transform.position-Vector3.right*0.4f-Vector3.up, -Vector2.up, float.PositiveInfinity,
                1 << LayerMask.NameToLayer("Terrain"))
        };
        return hit.Count(d => d.distance<2) > 2;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Any(contact => Vector2.Dot(contact.normal, Vector2.up) > 0.3f))
        {
            _jumps = MaxJumps;
            if (!_grounds.Contains(collision.transform)) _grounds.Add(collision.transform);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (_grounds.Contains(collision.transform)) _grounds.Remove(collision.transform);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Any(contact => Mathf.Abs(Vector2.Dot(contact.normal, Vector2.right)) > 0.7f))
        {
            _direction = 0;
        }
    }
}
