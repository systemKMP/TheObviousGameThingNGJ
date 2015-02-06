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
}

