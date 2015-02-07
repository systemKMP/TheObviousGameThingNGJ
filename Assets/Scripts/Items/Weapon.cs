using UnityEngine;
using System.Collections;

public class Weapon : ItemCore {

    public int ammo = 10; // -1 for infinity, 0 for item loss

    public Projectile projectile; //Item to instantiate on attack;

    public float attackInterval;



    protected bool readyForAttack = true;
    protected float attackTimer;

    void Update()
    {
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
        Destroy(this);
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
        
        insProj.SetOwner(weaponOwner);
    }
}
