using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : ItemCore {

    public int ammo = 10; // -1 for infinity, 0 for item loss

    public Projectile projectile; //Item to instantiate on attack;

    public float attackInterval;



    protected bool readyForAttack = true;
    protected float attackTimer;

    public override void Update() 
    {
        base.Update();
        if (!readyForAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0.0f)
            {
                readyForAttack = true;
            }
        }
    }

    public override void Use(int index)
    {
        if (readyForAttack)
        {
            FireProjectile(index);
            readyForAttack = false;
            attackTimer = attackInterval;
            ammo--;
            if (ammo == 0)
            {
                DestroyWeapon();
            }
        }

        base.Use(index);
    }



    protected virtual void DestroyWeapon()
    {
        weaponOwner.RemoveItem(this);
        Destroy(this.gameObject, 2.0f);

        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.transform.parent = null;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), weaponOwner.GetComponent<Collider2D>());
        used = true;
    }

    protected virtual void FireProjectile(int index)
    {

        var insProj = Instantiate(projectile, transform.position + transform.parent.localScale.x * Vector3.right * 0.7f, Quaternion.identity) as Projectile;
        if (index > 0){
            insProj.SetDirection(transform.parent.localScale.x * Vector2.right + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)));
        } else {
            insProj.SetDirection(transform.parent.localScale.x * Vector2.right);
        }
        Screenshaker.Shake(0.4f, Vector2.right * transform.parent.localScale.x);
        
        insProj.SetOwner(ref weaponOwner);
    }

    protected virtual IEnumerator SpawnProjectiles(List<ProjectileDef> projectilesToSpawn)
    {
        foreach (var proj in projectilesToSpawn)
        {
            var insProj = Instantiate(proj.projectileObject, proj.spawnLocation, Quaternion.identity) as Projectile;

                insProj.SetDirection(proj.velocity);

            insProj.SetOwner(ref proj.owner);

            

            if (Application.isPlaying)
                yield return new WaitForEndOfFrame();
        }
    }

}

public class ProjectileDef
{
    public Projectile projectileObject;
    public Vector3 spawnLocation;
    public Vector2 velocity;
    public PlayerCore owner;
}
