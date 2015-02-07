using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerCore : MonoBehaviour {

    public int PlayerID;
    
    public List<ItemCore> HeldItems;

    public int MaxHealth;
    public int CurrentHealth { get; set; }

    public int Armor { get; set; }

    public PlayerController Controller;

    void Start()
    {
        Controller = gameObject.GetComponent<PlayerController>();
        CurrentHealth = MaxHealth;

        for (int i = 0; i < HeldItems.Count; i++)
        {
            HeldItems[i] = Instantiate(HeldItems[i]) as ItemCore;
            HeldItems[i].GetComponent<Rigidbody2D>().isKinematic = true;
            HeldItems[i].GetComponent<Collider2D>().enabled = false;
            HeldItems[i].transform.parent = this.transform;
            HeldItems[i].transform.localScale = Vector3.one;
            HeldItems[i].transform.localPosition = Vector3.zero;

            HeldItems[i].SetOwner(this);
        }
    }


    public void Damage(int projectileDamage, int killerId)
    {
        CurrentHealth -= projectileDamage;
        if (CurrentHealth <= 0)
        {
            ScoreTracker.Instance.RecordKill(killerId, Controller.Index);
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        for (int i = 0; i < HeldItems.Count; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                HeldItems[i].GetComponent<Rigidbody2D>().isKinematic = false;
                HeldItems[i].GetComponent<Collider2D>().enabled = true;
                HeldItems[i].transform.parent = null;
                HeldItems[i].SetOwner(null);

                HeldItems.RemoveAt(i);
                i--;

            }


        
        }



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

            HeldItems.Add(item);
            item.SetOwner(this);



        }
    }

    public void RemoveItem(ItemCore item)
    {
        HeldItems.Remove(item);
    }

    public void UseItems()
    {
        for (int i = 0; i < HeldItems.Count; i++)
        {
            HeldItems[i].Use(i);
        }
    }
}

