using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerCore : MonoBehaviour {

    public int PlayerID;
    
    public List<ItemCore> heldItems;

    public int currentHealth;
    public int MaxHealth { get; set; }

    public int Armor { get; set; }

    public float MovementSpeed { get; set; }

    public float JumpStrength { get; set; }

    public int MaxJumps { get; set; }

    public ControllerController Controller;

    void Start()
    {
        Controller = gameObject.GetComponent<ControllerController>();
    }


    public void Damage(int projectileDamage, int killerId)
    {
        currentHealth -= projectileDamage;
        if (currentHealth <= 0)
        {
            ScoreTracker.Instance.RecordKill(killerId, Controller.Controller);
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var gObj = col.transform.gameObject;
        
        if (gObj.layer == 10) //if collides with player
        {
            var item = gObj.GetComponent<Weapon>();
            item.GetComponent<Rigidbody2D>().isKinematic = true;
            item.GetComponent<Collider2D>().enabled = false;
            item.transform.parent = this.transform;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            heldItems.Add(item);
            item.SetOwner(this);



        }
    }

    public void RemoveWeapon(ItemCore item)
    {
        heldItems.Remove(item);
    }

    public void UseItems()
    {
        for (int i = 0; i < heldItems.Count; i++)
        {
            heldItems[i].Use(i);
        }
    }
}

