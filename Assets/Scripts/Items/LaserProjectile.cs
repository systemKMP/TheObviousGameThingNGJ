﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserProjectile : Projectile
{
    public float Width;
    public AnimationCurve Curve;
    [Range(0, 1)]
    public float HitTime;

    private LineRenderer _line;
    private float _time;
    private Vector2 _direction;
    private bool _hit;

    public override void SetDirection(Vector2 direction, float angluarVelocity = 0.0f)
    {
        _direction = direction;
        _line = GetComponent<LineRenderer>();

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + direction * 3, direction, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Terrain"));
        var end = hit.normal == Vector2.zero
            ? transform.position + new Vector3(direction.x, direction.y) * 10000
            : new Vector3(hit.point.x, hit.point.y);

        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, end);
    }

    public void Update()
    {
        if (_direction != Vector2.zero)
        {
            var w = Curve.Evaluate(_time / survivalTime)*Width;
            _line.SetWidth(w, w);
            if (!_hit && _time / survivalTime > HitTime)
            {
                RaycastHit2D hit =
                    Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + _direction * 3, _direction, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Player"));
                if (hit.transform != null && (damageSelf || hit.transform != projectileOwner.transform))
                {
                    var core = hit.transform.GetComponent<PlayerCore>();
                    if(core!=null)core.Damage(projectileDamage, projectileOwner.Controller.Index);
                    Destroy(Instantiate(HitPrefab,hit.point,Quaternion.identity),2);
                }
                _hit = true;
            }
            _time += Time.deltaTime;
        }
    }
}
