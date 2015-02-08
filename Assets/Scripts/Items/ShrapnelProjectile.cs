using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShrapnelProjectile : Projectile {

    public Projectile shrapnelProjectile;

    public int bullets = 3;
    public float spreadAngle = 60;

    private bool collidedWithTerrain = false;
    private bool collidedWithPlayer = false;
    public bool splitOnPlayerCollision = false;

    private Vector2 intialVelocity;

    public float MaxRandomOffset;

    public float angularStrengthMax;
    

    protected override void Start()
    {
        base.Start();
        intialVelocity = rigidbody2D.velocity;
    }

    void OnDestroy()
    {

        if ((splitOnPlayerCollision && collidedWithPlayer) || !collidedWithPlayer || (collidedWithTerrain && explodeOnTerrain))
        {
            List<ProjectileDef> projectilesToSpawn = new List<ProjectileDef>();

            for (int i = 0; i < bullets; i++)
            {
                ProjectileDef pDef = new ProjectileDef() { projectileObject = shrapnelProjectile, spawnLocation = transform.position};

                float angle = (spreadAngle / 2 - spreadAngle / (bullets - 1) * i + Random.Range(-MaxRandomOffset, MaxRandomOffset)) * Mathf.Deg2Rad;

                Vector2 shotDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                if (intialVelocity.x < 0.0f)
                {
                   
                    shotDirection.x *= -1.0f;
                }

                pDef.velocity = shotDirection;

                pDef.owner = projectileOwner;

                projectilesToSpawn.Add(pDef);
            }

            foreach (var proj in projectilesToSpawn)
            {
                var insProj = Instantiate(proj.projectileObject, proj.spawnLocation, Quaternion.identity) as Projectile;

                var angVelocity = Random.Range(-angularStrengthMax, angularStrengthMax);
                insProj.SetDirection(proj.velocity, angVelocity);

                insProj.SetOwner(ref proj.owner);
                
            }
        }
        
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        var gObj = col.gameObject;
        if (gObj.layer == 8) //if collides with player
        {
            var playerCore = gObj.GetComponent<PlayerCore>();
            if ((damageSelf || playerCore != projectileOwner) && projectileOwner != null)
            {
                playerCore.Damage(projectileDamage, projectileOwner.Controller.Index);
                collidedWithPlayer = true;
                ProperDestroy();
            }
        }
        else if (gObj.layer == 11 && explodeOnTerrain)
        {
            collidedWithTerrain = true;
            ProperDestroy();
        }
    }
    
}
