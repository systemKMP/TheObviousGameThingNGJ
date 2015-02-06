using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;

    public virtual void SetDirection(Vector2 direction)
    {
        rigidbody2D.velocity = projectileSpeed * direction;
    }

}
