using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Movement Mover;

    public AnimationCurve WalkCurve;
    public float MaxWalkSpeed;
    public float WalkFloatiness;
    public float JumpForce;

    public Direction _direction;
    private float _walk;
    private float _time;

    public void Update()
    {
        if (_direction == Direction.None)
        {
            if (_time > 0)
            {
                _walk = WalkCurve.Evaluate(1 - _time / WalkFloatiness) * (_direction == Direction.Left ? -1 : 1) * MaxWalkSpeed;
                _time -= Time.deltaTime;
            }
            else
            {
                _walk = 0;
            }
        }
        else
        {
            if (_time <= WalkFloatiness)
            {
                _walk = WalkCurve.Evaluate(_time / WalkFloatiness) * (_direction == Direction.Left ? -1 : 1) * MaxWalkSpeed;
                _time += Time.deltaTime;
            }
            else
            {
                _walk = (_direction == Direction.Left ? -1 : 1) * MaxWalkSpeed;
            }
        }

        Mover.Move = new Vector2(_walk, 0);
    }

    public void Walk(Direction direction)
    {
        _direction = direction;
    }

    public void Jump()
    {
        
    }
}

public enum Direction
{
    Left, Right, None,
}
