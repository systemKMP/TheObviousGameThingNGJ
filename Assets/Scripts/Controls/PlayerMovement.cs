using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Movement Mover;

    public AnimationCurve WalkCurve;
    public float MaxWalkSpeed;
    public float WalkFloatiness;
    public float JumpForce;

    private float _direction;
    private float _prewalk;
    private float _walk;
    private float _time;
    private float _jump;

    public void Update()
    {
        if (Mover == null) { Debug.LogWarning("You haven't attached a movement script to me, moron"); return; }

        float desiredWalk = 0;
        if (_direction != 0)
        {
            desiredWalk = Mathf.Clamp(_direction, -MaxWalkSpeed, MaxWalkSpeed) * MaxWalkSpeed;
        }
        /*if (_time < WalkFloatiness) TODO I'll fix this part later, rasmus
        {
            _walk = Mathf.Lerp(_prewalk, desiredWalk, WalkCurve.Evaluate(_time / WalkFloatiness));
            _time += Time.deltaTime;
        }
        else
        {
            _walk = desiredWalk;
        }*/
        _walk = desiredWalk;

        Mover.Move = new Vector2(_walk, _jump);
        _jump = 0;
    }

    public void Walk(float direction)
    {
        if (_direction != direction) { _time = 0; _prewalk = _walk; }
        _direction = direction;
    }

    public void Jump()
    {
        _jump = JumpForce;
    }
}
