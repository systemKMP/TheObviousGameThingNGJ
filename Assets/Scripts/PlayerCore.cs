﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerCore : MonoBehaviour {

    public int PlayerID;
    
    public List<ItemCore> heldItems;

    public int currentHelath;
    public int MaxHealth { get; set; }

    public int Armor { get; set; }

    public float MovementSpeed { get; set; }

    public float JumpStrength { get; set; }

    public int MaxJumps { get; set; }

    void Update()
    {
        if (Input.GetButtonDown("Use Items"))
        {
            foreach (var item in heldItems)
            {
                item.Use();
            }
        }
    }

}

