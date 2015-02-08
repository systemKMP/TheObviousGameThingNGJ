﻿using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour
{
    public float Lerp = 0.3f;
    public float Player1Margin = 10;
    public float MorePlayerMargin = 4;
    public float SpeedCompensation = 10;
    public Vector2 Bounds = Vector2.one;
    private Vector3 _target;

    public void Start()
    {
        _target = transform.position;
    }

    public void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            var average = players.Aggregate(Vector3.zero, (prev, player) => prev + player.transform.position) / players.Length;
            _target = Vector3.Lerp(_target, new Vector3(average.x, average.y, transform.position.z), Lerp);
            transform.position = Vector3.Lerp(transform.position, _target, Lerp);

            var maxDist = players.Max(player => Vector2.Distance((player.transform.position + (Vector3)player.rigidbody2D.velocity*SpeedCompensation), transform.position));
            
            var cam = GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cam.orthographicSize =
                    Mathf.Lerp(cam.orthographicSize, maxDist + (players.Length == 1 ? Player1Margin : MorePlayerMargin), Lerp);
            }
        }
    }
}
