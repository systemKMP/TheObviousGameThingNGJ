using UnityEngine;
using System.Collections;

public class Weapon : ItemCore {

    public int bullets = -1; // -1 for infinity, 0 for item loss

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

    public override void Use()
    {
        if (readyForAttack)
        {
            FireProjectile();
            readyForAttack = false;
            attackTimer = attackInterval;
            bullets--;
            if (bullets == 0)
            {
                DestroyWeapon();
            }
        }
        
        base.Use();
    }



    protected virtual void DestroyWeapon()
    {
        weaponOwner.RemoveWeapon(this);
        Destroy(this);
    }

    protected virtual void FireProjectile()
    {
        var insProj = Instantiate(projectile, transform.position + transform.localScale.x * Vector3.right * 0.5f, Quaternion.identity) as Projectile;

        insProj.SetDirection(transform.localScale.x * Vector2.right);
        insProj.SetOwner(weaponOwner);
    }
}
