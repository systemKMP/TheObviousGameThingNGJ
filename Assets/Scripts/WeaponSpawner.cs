using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class WeaponSpawner : MonoBehaviour
{
    public List<ItemCore> Weapons = new List<ItemCore>();
    public List<float> Rates = new List<float>();
    public float SpawnMin = 2;
    public float SpawnMax = 4;

    public GameObject AttachEffect;
    public GameObject DropEffect;

    private float _time;
    private Weapon _currentWeapon;

    public void Update()
    {
        if (CanSpawn())
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
            }
            else
            {
                _time = Random.Range(SpawnMin, SpawnMax);

                var total = Rates.Sum();
                var result = Random.Range(0, total);
                var accum = 0f;
                for (int i = 0; i < Rates.Count; i++)
                {
                    if(Weapons[i] == null){Debug.LogWarning("Weapon is null");continue;}
                    accum += Rates[i];
                    if (result < accum)
                    {
                        _currentWeapon = (Weapon) Instantiate(Weapons[i], transform.position, transform.rotation);
                        if (AttachEffect != null)
                        {
                            var effect = (GameObject) Instantiate(AttachEffect, transform.position, transform.rotation);
                            effect.transform.parent = _currentWeapon.transform;
                            _currentWeapon.Effects.Add(effect);
                        }
                        if (DropEffect != null)
                        {
                            var hit = Physics2D.Raycast(transform.position, -Vector2.up, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Terrain"));
                            var effect = (GameObject)Instantiate(DropEffect, hit.point, Quaternion.identity);
                            _currentWeapon.Effects.Add(effect);
                        }
                        break;
                    }
                    
                }
            }
        }
        else
        {
            if (_currentWeapon == null || parentIsPlayer(_currentWeapon))
                _currentWeapon = null;
        }
    }

    private bool parentIsPlayer(Weapon weapon)
    {
        if (weapon == null || weapon.transform == null || weapon.transform.parent == null) return false;
        return weapon.transform.parent.GetComponent<PlayerMovement>() != null;
    }

    private bool CanSpawn()
    {
        return _currentWeapon == null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
