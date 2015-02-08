using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour
{
    public float Lerp = 0.3f;
    public float Player1Margin = 10;
    public float MorePlayerMargin = 4;
    public Vector2 Bounds = Vector2.one;
    private Vector3 _target;
    private Vector3 _origin;

    public void Start()
    {
        _origin = _target = transform.position;
    }

    public void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            var average = players.Aggregate(Vector3.zero, (prev, player) => prev + player.transform.position) / players.Length;
            _target = Vector3.Lerp(_target, new Vector3(average.x, average.y, transform.position.z), Lerp);
            transform.position = Vector3.Lerp(transform.position, _target, Lerp);

            var maxDist = players.Max(player => Vector2.Distance(player.transform.position, transform.position));
            var cam = GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cam.orthographicSize = Mathf.Min(Bounds.x / (cam.aspect * 2), Mathf.Lerp(cam.orthographicSize, maxDist + (players.Length == 1 ? Player1Margin : MorePlayerMargin), Lerp));

                if (transform.position.x < _origin.x - Bounds.x / 2 + cam.orthographicSize * cam.aspect)
                {
                    var pos = transform.position;
                    pos.x = _origin.x - Bounds.x / 2 + cam.orthographicSize * cam.aspect;
                    transform.position = pos;
                }
                if (transform.position.x > _origin.x + Bounds.x / 2 - cam.orthographicSize * cam.aspect)
                {
                    var pos = transform.position;
                    pos.x = _origin.x + Bounds.x / 2 - cam.orthographicSize * cam.aspect;
                    transform.position = pos;
                }
                if (transform.position.y < _origin.y - Bounds.y / 2 + cam.orthographicSize)
                {
                    var pos = transform.position;
                    pos.y = _origin.y - Bounds.y / 2 + cam.orthographicSize;
                    transform.position = pos;
                }
                if (transform.position.y > _origin.y + Bounds.y / 2 - cam.orthographicSize)
                {
                    var pos = transform.position;
                    pos.y = _origin.y + Bounds.y / 2 - cam.orthographicSize;
                    transform.position = pos;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Bounds);
    }
}
