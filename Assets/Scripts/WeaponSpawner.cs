using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class WeaponSpawner : MonoBehaviour
{
    public List<Weapon> Weapons = new List<Weapon>();
    public List<float> Rates = new List<float>();
    public float SpawnMin = 2;
    public float SpawnMax = 4;

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
                    accum += Rates[i];
                    if (result < accum)
                    {
                        _currentWeapon = (Weapon) Instantiate(Weapons[i], transform.position, transform.rotation);
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
