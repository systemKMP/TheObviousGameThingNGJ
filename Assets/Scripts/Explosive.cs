using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosive : Projectile {

    public float collisionValidityTimer = 0.05f;
    private float currentTimer;

    private List<PlayerCore> damagedPlayers;

    protected override void Start()
    {
        base.Start();
        currentTimer = collisionValidityTimer;
        damagedPlayers = new List<PlayerCore>();
        if (!Application.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();
        currentTimer -= Time.deltaTime;
        damagedPlayers = new List<PlayerCore>(); ;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var gObj = col.gameObject;
        //Debug.Log(gObj.layer);
        if (gObj.layer == 8)
        {
            var playerCore = gObj.GetComponent<PlayerCore>();
            if (!damagedPlayers.Contains(playerCore))
            {
                damagedPlayers.Add(playerCore);
                if (playerCore != null && (damageSelf || playerCore != projectileOwner) && currentTimer >= 0.0f)
                {
                    //Debug.Log(projectileOwner);
                    playerCore.Damage(projectileDamage, projectileOwner.Controller.Index);
                    //Destroy(this.gameObject);
                }
            }
        }
    }

    

}
