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

    public void Awake()
    {
        if (_instance == null) _instance = this;
        _instance._spawnPoints.Add(this);
    }

    public void Update()
    {
        if (_instance != this) return;
        var players = GameObject.FindGameObjectsWithTag("Player");
        var averageplayer = players.Aggregate(Vector3.zero, (pre, go) => pre + go.transform.position) / players.Length;
        var maxDist = _spawnPoints.Max(spawner => Vector3.Distance(spawner.transform.position, averageplayer));

        for (int i = 1; i <= 8; i++)
        {
            if (_players.ContainsKey(i) && _players[i] == null) _players.Remove(i);
            if (_players.ContainsKey(i) || !Input.GetKey("joystick " + i + " button 2") || !UI.Instance.CanSpawn) continue;

            var player = (PlayerMovement)Instantiate(PlayerPrefab, 
                (_spawnPoints.SingleOrDefault(spawner => Vector3.Distance(spawner.transform.position, averageplayer) == maxDist)??
                 _spawnPoints[Random.Range(0,_spawnPoints.Count)]).transform.position,
                Quaternion.identity);
            var controller = player.GetComponent<JoystickController>() ??
                             player.gameObject.AddComponent<JoystickController>();
            controller.Player = player;
            controller.Index = i;
            _players[i] = player;
            Screenshaker.Shake(1, Vector2.up);
            ScoreTracker.Instance.RegisterPlayer(i, player.GetComponent<PlayerCore>());
            GetComponent<AudioSource>().clip = SpawnClips[Random.Range(0, SpawnClips.Length)];
            GetComponent<AudioSource>().Play();
            if(SpawnEffect.Length >= i) Destroy(Instantiate(SpawnEffect[i-1],player.transform.position,Quaternion.identity),4);
        }

        if (_players.ContainsKey(0) && _players[0] == null) _players.Remove(0);
        if (!_players.ContainsKey(0) && Input.GetKey(KeyCode.Space) && UI.Instance.CanSpawn)
        {
            var keyboardplayer = (PlayerMovement)Instantiate(PlayerPrefab,
                (_spawnPoints.SingleOrDefault(spawner => Vector3.Distance(spawner.transform.position, averageplayer) == maxDist) ??
                 _spawnPoints[Random.Range(0, _spawnPoints.Count)]).transform.position,
                Quaternion.identity);
            var keyboardcontroller = keyboardplayer.GetComponent<KeyboardController>() ??
                                     keyboardplayer.gameObject.AddComponent<KeyboardController>();
            keyboardcontroller.Player = keyboardplayer;
            keyboardcontroller.WalkInputAxis = "Horizontal";
            keyboardcontroller.JumpInput = KeyCode.W;
            _players[0] = keyboardplayer;
            Screenshaker.Shake(1, Vector2.up);
            ScoreTracker.Instance.RegisterPlayer(0, keyboardplayer.GetComponent<PlayerCore>());
            GetComponent<AudioSource>().clip = SpawnClips[Random.Range(0, SpawnClips.Length)];
            GetComponent<AudioSource>().Play();
            if (SpawnEffect.Length > 0) Destroy(Instantiate(SpawnEffect[0], keyboardplayer.transform.position, Quaternion.identity), 4);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
