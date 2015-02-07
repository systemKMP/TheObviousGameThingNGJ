using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpreadShotWeapon : Weapon {

    [Range(2, 10)]
    public int bullets = 3;
    public float spreadAngle = 60;


    protected override void FireProjectile(int index)
    {
        for (int i = 0; i < bullets; i++)
        {
            var insProj = Instantiate(projectile, transform.position + transform.parent.localScale.x * Vector3.right * 2.0f, Quaternion.identity) as Projectile;
            float angle = (spreadAngle / 2 - spreadAngle / (bullets-1) * i) * Mathf.Deg2Rad;
            Debug.Log(angle);
            Vector2 shotDirection = new Vector2(Mathf.Acos(angle), Mathf.Asin(angle)) * transform.parent.localScale.x;
            if (index > 0)
            {
                shotDirection += new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            }
            else
            {
                shotDirection += Vector2.right + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                
            }
            insProj.SetDirection(shotDirection);
            Screenshaker.Shake(0.4f, Vector2.right * transform.parent.localScale.x);
             
            insProj.SetOwner(weaponOwner);
        }
    }

}
