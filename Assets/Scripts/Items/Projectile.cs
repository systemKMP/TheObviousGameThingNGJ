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

    public GameObject trail;
    public GameObject destroyEffect;



    protected virtual void Start()
    {
        
        Destroy(this.gameObject, survivalTime);
    }

    public virtual void SetDirection(Vector2 direction, float angularVelocity = 0.0f)
    {
        rigidbody2D.velocity = projectileSpeed * direction.normalized;
        rigidbody2D.angularVelocity = angularVelocity;

        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1.0f;
            transform.localScale = scale;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D col)
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


    public virtual void OnDestroy()
    {
        if (HitPrefab != null)
        {
            Instantiate(HitPrefab, transform.position, Quaternion.identity);
        }
        if (trail != null)
        {
            trail.transform.parent = null;
        }
    }
}
