using UnityEngine;
using System.Collections;

public class AngleThrowWeapon : Weapon {

    public float thorwAngle;
    public float randomRange;

    protected override void FireProjectile(int index)
    {
        var insProj = Instantiate(projectile, transform.position + transform.parent.localScale.x * Vector3.right * 1.2f + Vector3.up * 1.2f , Quaternion.identity) as Projectile;


        Physics2D.IgnoreCollision(insProj.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        float angle = (thorwAngle + Random.Range(-randomRange, randomRange)) * Mathf.Deg2Rad;

     
        Vector2 shotDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        shotDirection.x *= transform.parent.localScale.x;

        

        insProj.SetDirection(shotDirection);

        insProj.SetOwner(ref weaponOwner);

        Screenshaker.Shake(0.4f, Vector2.right * transform.parent.localScale.x);

        
    }

}
