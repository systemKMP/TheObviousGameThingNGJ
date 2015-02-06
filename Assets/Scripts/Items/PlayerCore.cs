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

    void Update()
    {
        //TODO: if (Input.GetButtonDown("Use Items"))

        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var item in heldItems)
            {
                item.Use();
            }
        }
    }


    internal void Damage(int projectileDamage)
    {
        currentHealth -= projectileDamage;
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var gObj = col.transform.gameObject;
        Debug.Log(gObj.layer);
        
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
}

