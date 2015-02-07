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

    protected override void Update()
    {
        base.Update();
        currentTimer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var gObj = col.gameObject;
        //Debug.Log(gObj.layer);
        if (gObj.layer == 8)
        {
            var playerCore = gObj.GetComponent<PlayerCore>();
            if (playerCore != null && (damageSelf || playerCore != projectileOwner) && currentTimer >= 0.0f)
            {
                //Debug.Log(projectileOwner);
                playerCore.Damage(projectileDamage, projectileOwner.Controller.Index);
                //Destroy(this.gameObject);
            }
        }
    }

    

}
