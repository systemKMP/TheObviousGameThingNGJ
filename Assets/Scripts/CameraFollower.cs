using System.Linq;
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
    private Vector3 _origin;
    private bool _viewToggle;

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

            var maxDist = players.Max(player => Vector2.Distance((player.transform.position + (Vector3)player.rigidbody2D.velocity*SpeedCompensation), transform.position));
            var maxDistY = players.Max(player => Mathf.Abs((player.transform.position + (Vector3)player.rigidbody2D.velocity * SpeedCompensation).y - transform.position.y));

            var cam = GetComponentInChildren<Camera>();
            if (cam != null)
            {
                if (maxDistY > cam.orthographicSize) _viewToggle = true;
                if (maxDistY < cam.orthographicSize*0.65f) _viewToggle = false;

                cam.orthographicSize =
                    Mathf.Lerp(cam.orthographicSize, Mathf.Min(_viewToggle ? float.PositiveInfinity : Bounds.x / (cam.aspect * 2), 
                        maxDist + (players.Length == 1 ? Player1Margin : MorePlayerMargin)), Lerp);

                if (!_viewToggle)
                {
                    if (_target.x < _origin.x - Bounds.x / 2 + cam.orthographicSize * cam.aspect)
                    {
                        _target.x = _origin.x - Bounds.x / 2 + cam.orthographicSize * cam.aspect;
                    }
                    if (transform.position.x > _origin.x + Bounds.x / 2 - cam.orthographicSize * cam.aspect)
                    {
                        _target.x = _origin.x + Bounds.x / 2 - cam.orthographicSize * cam.aspect;
                    }
                }
                if (transform.position.y < _origin.y - Bounds.y / 2 + cam.orthographicSize)
                {
                    _target.y = _origin.y - Bounds.y / 2 + cam.orthographicSize;
                }
                if (transform.position.y > _origin.y + Bounds.y / 2 - cam.orthographicSize)
                {
                    _target.y = _origin.y + Bounds.y / 2 - cam.orthographicSize;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Bounds);
    }
}
