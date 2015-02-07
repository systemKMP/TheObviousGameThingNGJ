using UnityEngine;
using System.Collections;

public class Explosive : Projectile {

    public float collisionValidityTimer = 0.05f;
    private float currentTimer;

    protected override void Start()
    {
        base.Start();
        currentTimer = collisionValidityTimer;
    }

    void Update()
    {
        currentTimer -= Time.deltaTime;
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        var gObj = col.gameObject;
        if (gObj.layer == 8)
        {
            var playerCore = gObj.GetComponent<PlayerCore>();
            if ((damageSelf || playerCore != projectileOwner) && projectileOwner != null && currentTimer >= 0.0f)
            {
                playerCore.Damage(projectileDamage, projectileOwner.Controller.Index);
                Destroy(this.gameObject);
            }
        }
    }

    

}
