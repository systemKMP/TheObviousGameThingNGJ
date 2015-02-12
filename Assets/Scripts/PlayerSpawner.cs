using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner _instance;
    private readonly List<PlayerSpawner> _spawnPoints = new List<PlayerSpawner>();
    private readonly Dictionary<int, PlayerMovement> _players = new Dictionary<int, PlayerMovement>();

    public PlayerMovement PlayerPrefab;
    public AudioClip[] SpawnClips;
    public GameObject[] SpawnEffect;

    private Vector3 averageplayer;
    private float maxDist;

    public void Awake()
    {
        if (_instance == null) _instance = this;
        _instance._spawnPoints.Add(this);
    }

    public void Update()
    {
        if (_instance != this) return;
        PlayerMovement player;
        PlayerController controller;
        var players = GameObject.FindGameObjectsWithTag("Player");
        averageplayer = players.Aggregate(Vector3.zero, (pre, go) => pre + go.transform.position) / players.Length;
        maxDist = _spawnPoints.Max(spawner => Vector3.Distance(spawner.transform.position, averageplayer));

        for (int i = 1; i <= 4; i++)
        {
            if (_players.ContainsKey(i) && _players[i] == null) _players.Remove(i);
            if (_players.ContainsKey(i) || !Input.GetKey("joystick " + i + " button 2") || !UI.Instance.CanSpawn) continue;

            player = (PlayerMovement)Instantiate(PlayerPrefab);
            controller = player.GetComponent<JoystickController>() ??
                             player.gameObject.AddComponent<JoystickController>();
            if (player.GetComponent<KeyboardController>()) Destroy(player.GetComponent<KeyboardController>());

            var index = i;
            while (ScoreTracker.Instance.Scores.Any(score => score.playerId == index))
            {
                index++;
                if (index > 4) index -= 4;
            }
            Spawn(i, index, controller);
        }

        if (_players.ContainsKey(0) && _players[0] == null) _players.Remove(0);
        if (!_players.ContainsKey(0) && Input.GetKey(KeyCode.X) && UI.Instance.CanSpawn)
        {
            player = (PlayerMovement)Instantiate(PlayerPrefab);
            controller = player.GetComponent<KeyboardController>() ??
                                     player.gameObject.AddComponent<KeyboardController>();
            if (player.GetComponent<JoystickController>()) Destroy(player.GetComponent<JoystickController>());
            Spawn(0, new[] { 1, 2, 3, 4 }.First(i => ScoreTracker.Instance.Scores.All(score => score.playerId != i)), controller);
        }
    }

    private void Spawn(int index, int slot, PlayerController controller)
    {
        var player = controller.GetComponent<PlayerMovement>();
        controller.transform.position =
            (_spawnPoints.SingleOrDefault(
                spawner => Vector3.Distance(spawner.transform.position, averageplayer) == maxDist) ??
             _spawnPoints[Random.Range(0, _spawnPoints.Count)]).transform.position;
        controller.Player = player;
        controller.Index = slot;
        _players[index] = player;
        Screenshaker.Shake(1, Vector2.up);
        ScoreTracker.Instance.RegisterPlayer(slot, index, player.GetComponent<PlayerCore>());
        GetComponent<AudioSource>().clip = SpawnClips[Random.Range(0, SpawnClips.Length)];
        GetComponent<AudioSource>().Play();
        if (SpawnEffect.Length >= slot) Destroy(Instantiate(SpawnEffect[Mathf.Clamp(slot - 1, 0, SpawnEffect.Length)], player.transform.position, Quaternion.identity), 4);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
