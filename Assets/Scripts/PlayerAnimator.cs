using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeState(string spriteName)
    {
        _renderer.sprite = Sprites[spriteName];
    }
}
