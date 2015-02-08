using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour
{
    public float Lerp = 0.3f;
    public float PlayerMarginX = 20;
    public float PlayerMarginY = 20;

    public Vector2 Bounds = Vector2.one;
    private Vector3 _target;
    public int minSize = 100;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float horCamDif;
    public float verCamDif;

    public float targetOrth;

    public void Start()
    {
        _target = transform.position;
    }

    public void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            var cam = GetComponentInChildren<Camera>();

            minX = players.Min(n => n.transform.position.x);
            maxX = players.Max(n => n.transform.position.x);
            minY = players.Min(n => n.transform.position.y);
            maxY = players.Max(n => n.transform.position.y);

            horCamDif = cam.orthographicSize * cam.aspect * 2;
            verCamDif = cam.orthographicSize * 2;

            targetOrth = minSize;

            if ((maxX - minX) + PlayerMarginX < horCamDif)
            {
                targetOrth = ((maxX - minX) / cam.aspect) / 2.0f + PlayerMarginX;
            }

            if ((maxY - minY) + PlayerMarginY < verCamDif)
            {
                Debug.Log("vert fix");
                targetOrth = (maxY - minY) / 2.0f + PlayerMarginY;
            }

            _target = Vector3.Lerp(_target, new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, transform.position.z), Lerp);

            transform.position = Vector3.Lerp(transform.position, _target, Lerp * Time.deltaTime);

            var maxDist = players.Max(player => Vector2.Distance((player.transform.position + (Vector3)player.rigidbody2D.velocity), transform.position));


            if (cam != null)
            {
                cam.orthographicSize =
                    Mathf.Lerp(cam.orthographicSize, targetOrth, Time.deltaTime * Lerp);
                cam.orthographicSize = minSize > cam.orthographicSize ? minSize : cam.orthographicSize;
            }
        }
    }
}
