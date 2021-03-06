﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ItemCore : MonoBehaviour
{
    [HideInInspector]
    public bool used = false;

    protected PlayerCore weaponOwner;
    public float HoverHeight;
    public List<GameObject> Effects = new List<GameObject>();

    public virtual void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, float.PositiveInfinity,
            1 << LayerMask.NameToLayer("Terrain"));
        if (hit.normal != Vector2.zero)
        {
            var dist = Mathf.Max(HoverHeight - hit.distance, 0);
            rigidbody2D.AddForce(Vector2.up * dist * rigidbody2D.mass * 8);
        }
    }

    public virtual void Use(int index)
    {

    }

    public virtual void SetOwner(PlayerCore weaponOwner)
    {
        this.weaponOwner = weaponOwner;
        foreach (var effect in Effects)
        {
            Destroy(effect);
        }
    }
}
