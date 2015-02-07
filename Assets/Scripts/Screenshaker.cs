using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Screenshaker : MonoBehaviour
{
    public static void Shake(float amount, Vector2 direction)
    {
        foreach (var screenshaker in _list)
        {
            screenshaker.ShakeIt(amount, direction);
        }
    }
    private static readonly List<Screenshaker> _list = new List<Screenshaker>();

    public Camera Camera;
    private Vector2 _direction;
    private float _amount;

    public void Start()
    {
        if (Camera.transform == transform)
        {
            Destroy(this);
            return;
        }
        _list.Add(this);
    }

    public void Update()
    {
        if (_amount > 0)
        {
            _amount -= Time.deltaTime;
            Camera.transform.localPosition = new Vector3(_direction.x, _direction.y) * _amount * Mathf.Cos(Time.time * 100);
        }
    }

    private void ShakeIt(float amount, Vector2 direction)
    {
        _amount += amount;
        _direction = direction == Vector2.zero 
            ? direction 
            : Vector2.Lerp(_direction, direction.normalized, 0.5f).normalized;
    }
}
