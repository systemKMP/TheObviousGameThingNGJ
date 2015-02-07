using UnityEngine;
using System.Collections;

public class AngleThrowWeapon : Weapon {

    public float thorwAngle;
    public float randomRange;
    public float rotationalAngle;

    protected override void FireProjectile(int index)
    {
        var insProj = Instantiate(projectile, transform.position + transform.parent.localScale.x * Vector3.right * 0.7f, Quaternion.identity) as Projectile;

        float angle = (thorwAngle + Random.Range(-randomRange, randomRange)) * Mathf.Deg2Rad;

        Vector2 shotDirection = new Vector2(Mathf.Acos(angle), Mathf.Asin(angle));
        shotDirection.x *= transform.parent.localScale.x;

        insProj.SetDirection(shotDirection);

        Screenshaker.Shake(0.4f, Vector2.right * transform.parent.localScale.x);

        insProj.SetOwner(weaponOwner);
    }

}
