using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner _instance;
    private static readonly List<PlayerSpawner> SpawnPoints = new List<PlayerSpawner>();
    private static readonly Dictionary<int, PlayerMovement> Players = new Dictionary<int, PlayerMovement>();

    public PlayerMovement PlayerPrefab;

    public void Awake()
    {
        if (_instance == null) _instance = this;
        SpawnPoints.Add(this);
    }

    public void Update()
    {
        if (_instance != this) return;

        for (int i = 1; i <= 8; i++)
        {
            if (Players.ContainsKey(i) && Players[i] == null) Players.Remove(i);
            if (Players.ContainsKey(i) || !Input.GetKey("joystick " + i + " button 0")) continue;

            var player = (PlayerMovement)Instantiate(PlayerPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position,
                Quaternion.identity);
            var controller = player.GetComponent<JoystickController>() ??
                             player.gameObject.AddComponent<JoystickController>();
            controller.Player = player;
            controller.Index = i;
            Players[i] = player;
        }
        
        if (Players.ContainsKey(0) && Players[0] == null) Players.Remove(0);
        if (!Players.ContainsKey(0) && Input.GetKey(KeyCode.Space))
        {
            var keyboardplayer =
                (PlayerMovement)
                    Instantiate(PlayerPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position,
                        Quaternion.identity);
            var keyboardcontroller = keyboardplayer.GetComponent<KeyboardController>() ??
                                     keyboardplayer.gameObject.AddComponent<KeyboardController>();
            keyboardcontroller.Player = keyboardplayer;
            keyboardcontroller.WalkInputAxis = "Horizontal";
            keyboardcontroller.JumpInput = KeyCode.W;
            Players[0] = keyboardplayer;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
