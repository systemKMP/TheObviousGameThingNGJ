using UnityEngine;
using System.Collections;

public class ItemCore : MonoBehaviour {

    protected PlayerCore weaponOwner;

    public virtual void Use(int index)
    {
        
    }

    public virtual void SetOwner(PlayerCore weaponOwner)
    {
        this.weaponOwner = weaponOwner;
    }
}
