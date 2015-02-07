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



    protected virtual void Start()
    {

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
                ProperDestroy();
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
    }

    public void FreeTrail()
    {
        Debug.Log(trail);
        if (trail != null)
        {
            Debug.Log("freeing");
            trail.transform.parent = null;
            //trail.emissionRate = 0.0f;
            Destroy(trail.gameObject, 1.0f);
        }
    }
}
