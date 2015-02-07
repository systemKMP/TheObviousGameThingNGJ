﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerCore : MonoBehaviour
{

    public int PlayerID;

    public AudioSource Audio;

    public List<ItemCore> HeldItems;

    public int MaxHealth;
    public int CurrentHealth { get; set; }

    public int Armor { get; set; }

    public PlayerController Controller;

    public AudioClip[] HitClips = new AudioClip[0];

    private Color _originalColor;
    private SpriteRenderer _sprite;
    private float _hit;

    void Start()
    {
        Controller = gameObject.GetComponent<PlayerController>();
        CurrentHealth = MaxHealth;
        _sprite = GetComponent<SpriteRenderer>();
        _originalColor = _sprite.color;

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

    public virtual void Update()
    {
        if (_hit > 0)
        {
            _sprite.color = Color.Lerp(_originalColor, Color.black, _hit / 0.5f);
            _hit -= Time.deltaTime;
        }
        else
        {
            _sprite.color = _originalColor;
        }

        if (Input.GetKey("joystick " + Controller.Index + " button 6")) Damage(CurrentHealth, Controller.Index);
    }

    public void Damage(int projectileDamage, int killerId)
    {
        UI.Instance.Hit[Controller.Index] = _hit = 0.5f;
        Audio.clip = HitClips[Random.Range(0, HitClips.Length)];
        Audio.Play();

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
            if (!item.used)
            {
                item.GetComponent<Rigidbody2D>().isKinematic = true;
                item.GetComponent<Collider2D>().enabled = false;
                item.transform.parent = this.transform;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;

                HeldItems.Add(item);
                item.SetOwner(this);
            }


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

