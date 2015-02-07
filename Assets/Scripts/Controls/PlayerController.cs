using UnityEngine;
using System.Collections;

public abstract class PlayerController : MonoBehaviour {

    public PlayerMovement Player;
    public virtual int Index { get; set; }

    protected PlayerCore Core;

    public void Start()
    {
        Core = gameObject.GetComponent<PlayerCore>();
    }
}
