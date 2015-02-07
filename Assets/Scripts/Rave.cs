using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class Rave : MonoBehaviour
{
    public float PartyTime = 5;
    public float Shake = 10;
    public LaserProjectile Laser;

    private float _time;
    private bool _started;

    public void Update()
    {
        if (!_started)
        {
            if (
                ScoreTracker.Instance.Scores.Select(score => score.playerId)
                    .All(id => Input.GetKey("joystick " + id + " button 3"))
                && ScoreTracker.Instance.Scores.Count > 0)
            {
                _time += Time.deltaTime;
                Screenshaker.Shake((_time / PartyTime) * Shake, new Vector2(Random.value, Random.value).normalized);
                if (_time >= PartyTime)
                {
                    _started = true;
                }
            }
            else
            {
                _time = 0;
            }
        }
        else
        {
            var laser = (LaserProjectile)Instantiate(Laser, transform.position + new Vector3((Random.value - 0.5f) * Camera.main.orthographicSize, (Random.value - 0.5f) * Camera.main.orthographicSize), Quaternion.identity);
            laser.SetDirection(new Vector2(Random.value - 0.5f, Random.value - 0.5f));
            Destroy(laser, laser.survivalTime);
        }
    }
}
