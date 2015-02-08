using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject HitPrefab;
    public float projectileSpeed;
    public float survivalTime = 1.0f;
    private float destructionTimer;
    private bool destroying = false;

    public int projectileDamage = 1;

    public bool damageSelf = false;

    protected PlayerCore projectileOwner;

    public ParticleSystem trail;
    public GameObject destroyEffect;

    public bool explodeOnTerrain = false;



    protected virtual void Start()
    {
        survivalTime += Random.Range(0.0f, 1.0f);
        StartDestruction();
        if (trail != null)
        {
            trail = Instantiate(trail, transform.position, Quaternion.identity) as ParticleSystem;
            trail.gameObject.transform.parent = transform;
        }
    }

    protected virtual void Update()
    {
        if (destroying)
        {
            destructionTimer -= Time.deltaTime;
            if (destructionTimer < 0)
            {
                ProperDestroy();
            }
        }
    }

    public void ProperDestroy(){
        if (HitPrefab != null)
        {
            Instantiate(HitPrefab, transform.position, Quaternion.identity);
        }
        if (destroyEffect != null)
        {
            Destroy(Instantiate(destroyEffect, transform.position, Quaternion.identity), 1.0f);
        }
        FreeTrail();
        Destroy(gameObject);
    }

    public void StartDestruction()
    {
        destroying = true;
        destructionTimer = survivalTime;

    }

    public virtual void SetDirection(Vector2 direction, float angularVelocity = 0.0f)
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = projectileSpeed * direction.normalized;
            rigidbody2D.angularVelocity = angularVelocity;
        }

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
                ProperDestroy();
            }
        }
    }

    public void SetOwner(ref PlayerCore owner)
    {
        
        this.projectileOwner = owner;
        if (!damageSelf)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), owner.GetComponent<Collider2D>());
        }
    }

    public void FreeTrail()
    {
        if (trail != null)
        {
            trail.transform.parent = null;
            trail.emissionRate = 0.0f;
            Destroy(trail.gameObject, 1.0f);
        }
    }
}
