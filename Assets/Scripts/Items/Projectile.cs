using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject HitPrefab;
    public float projectileSpeed;
    public float survivalTime = 1.0f;

    public int projectileDamage = 1;

    public bool damageSelf = false;

    protected PlayerCore projectileOwner;

    void Start()
    {
        
        Destroy(this.gameObject, survivalTime);
    }

    public virtual void SetDirection(Vector2 direction)
    {
        rigidbody2D.velocity = projectileSpeed * direction.normalized;
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1.0f;
            transform.localScale = scale;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        var gObj = col.gameObject;
        if (gObj.layer == 8) //if collides with player
        {
            var playerCore = gObj.GetComponent<PlayerCore>();
            if ((damageSelf || playerCore != projectileOwner) && projectileOwner != null)
            {
                playerCore.Damage(projectileDamage, projectileOwner.Controller.Index);
                Destroy(this.gameObject);
            }
        }
    }

    public void SetOwner(PlayerCore projectileOwner)
    {
        this.projectileOwner = projectileOwner;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), projectileOwner.GetComponent<Collider2D>());
    }
}
