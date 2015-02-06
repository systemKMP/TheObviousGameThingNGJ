using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public Vector2 Move = Vector3.zero;

    private Rigidbody2D _rigidbody2D;

    public virtual void Awake()
    {
        _rigidbody2D = rigidbody2D;
    }

    public virtual void FixedUpdate()
    {
        if (Move.x != 0)
            _rigidbody2D.AddForce(new Vector2(Move.x - _rigidbody2D.velocity.x * _rigidbody2D.mass, 0), ForceMode2D.Impulse);
        if (Move.y != 0)
            _rigidbody2D.AddForce(new Vector2(0, Move.y - _rigidbody2D.velocity.y * _rigidbody2D.mass), ForceMode2D.Impulse);
    }
}
