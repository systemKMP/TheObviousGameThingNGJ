using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    public float survivalTime = 1.0f;

    public int projectileDamage = 1;

    private PlayerCore projectileOwner;

    void Start()
    {
        Destroy(this.gameObject, survivalTime);
    }

    public virtual void SetDirection(Vector2 direction)
    {
        rigidbody2D.velocity = projectileSpeed * direction;
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        var gObj = col.gameObject;
        if (gObj.layer == 1 << 8) //if collides with player
        {
            gObj.GetComponent<PlayerCore>().Damage(projectileDamage);
        }
    }

}
