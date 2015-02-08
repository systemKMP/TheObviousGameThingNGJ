using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner _instance;
    private static readonly List<PlayerSpawner> SpawnPoints = new List<PlayerSpawner>();
    private static readonly Dictionary<int, PlayerMovement> Players = new Dictionary<int, PlayerMovement>();

    public PlayerMovement PlayerPrefab;
    public AudioClip[] SpawnClips;
    public GameObject[] SpawnEffect;

    public void Awake()
    {
        if (_instance == null) _instance = this;
        SpawnPoints.Add(this);
    }

    public void Update()
    {
        if (_instance != this) return;
        var players = GameObject.FindGameObjectsWithTag("Player");
        var averageplayer = players.Aggregate(Vector3.zero, (pre, go) => pre + go.transform.position) / players.Length;
        var maxDist = SpawnPoints.Max(spawner => Vector3.Distance(spawner.transform.position, averageplayer));

        for (int i = 1; i <= 8; i++)
        {
            if (Players.ContainsKey(i) && Players[i] == null) Players.Remove(i);
            if (Players.ContainsKey(i) || !Input.GetKey("joystick " + i + " button 2")) continue;

            var player = (PlayerMovement)Instantiate(PlayerPrefab, 
                (SpawnPoints.SingleOrDefault(spawner => Vector3.Distance(spawner.transform.position, averageplayer) == maxDist)??
                 SpawnPoints[Random.Range(0,SpawnPoints.Count)]).transform.position,
                Quaternion.identity);
            var controller = player.GetComponent<JoystickController>() ??
                             player.gameObject.AddComponent<JoystickController>();
            controller.Player = player;
            controller.Index = i;
            Players[i] = player;
            Screenshaker.Shake(1, Vector2.up);
            ScoreTracker.Instance.RegisterPlayer(i, player.GetComponent<PlayerCore>());
            GetComponent<AudioSource>().clip = SpawnClips[Random.Range(0, SpawnClips.Length)];
            GetComponent<AudioSource>().Play();
            if(SpawnEffect.Length > i) Destroy(Instantiate(SpawnEffect[i],player.transform.position,Quaternion.identity),4);
        }

        if (Players.ContainsKey(0) && Players[0] == null) Players.Remove(0);
        if (!Players.ContainsKey(0) && Input.GetKey(KeyCode.Space))
        {
            var keyboardplayer = (PlayerMovement)Instantiate(PlayerPrefab,
                (SpawnPoints.SingleOrDefault(spawner => Vector3.Distance(spawner.transform.position, averageplayer) == maxDist) ??
                 SpawnPoints[Random.Range(0, SpawnPoints.Count)]).transform.position,
                Quaternion.identity);
            var keyboardcontroller = keyboardplayer.GetComponent<KeyboardController>() ??
                                     keyboardplayer.gameObject.AddComponent<KeyboardController>();
            keyboardcontroller.Player = keyboardplayer;
            keyboardcontroller.WalkInputAxis = "Horizontal";
            keyboardcontroller.JumpInput = KeyCode.W;
            Players[0] = keyboardplayer;
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
