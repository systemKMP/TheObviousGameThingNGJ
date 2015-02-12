using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UI : MonoBehaviour
{
    public float BlinkSpeed = 1.5f;
    public float LogoSize = 115;
    public float ShakeAmount = 50;
    public float VictoryTIme = 15;
    public float DefaultTImer = 90;
    public AudioClip VictoryClip;

    public Dictionary<int, float> Hit = new Dictionary<int, float>();
    public Dictionary<int, float> CountDown = new Dictionary<int, float>();

    public static UI Instance;
    public static float Timer;

    public bool CanSpawn { get { return _victory == 0 && ScoreTracker.Instance.Scores.Count < 4; } }

    private int _victor;
    private float _victory;
    private float _timer;
    private Dictionary<int, Color> originalColors = new Dictionary<int, Color>();
    private Dictionary<int, Vector2> originalPosis = new Dictionary<int, Vector2>();

    public void Start()
    {
        Instance = this;
        Timer = DefaultTImer;
    }

    public void Update()
    {
        var players = new Dictionary<int, PlayerScore>();

        foreach (var score in ScoreTracker.Instance.Scores)
        {
            players[score.playerId] = score;
        }

        var title = FindChild("Title", transform) == null
            ? null
            : FindChild("Title", transform).GetComponent<Image>();
        var hint = FindChild("Hint", transform) == null
            ? null
            : FindChild("Hint", transform).GetComponent<Text>();
        var timeText = FindChild("Time", transform) == null
            ? null
            : FindChild("Time", transform).GetComponentInChildren<Text>();
        var winText = FindChild("Win", transform) == null
            ? null
            : FindChild("Win", transform).GetComponentInChildren<Text>();
        if (title != null)
        {
            if (hint != null)
            {
                hint.rectTransform.localScale = Vector2.one * (0.8f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.2f));
            }
            title.rectTransform.localScale = Vector3.Lerp(title.rectTransform.localScale,
                ShowTitle() ? Vector3.one * (0.9f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.1f)) : Vector3.zero, 0.02f); // TODO pretty transition
        }

        if (winText != null)
        {
            winText.rectTransform.parent.localScale = _victory > 0
                ? new Vector2(1 / 0.11f, 1 / 0.73f) * (0.8f + Mathf.Abs(Mathf.Cos(Time.time * 3) * 0.2f))
                : Vector2.zero;
            while (_victor > 4) _victor -= 4;
            if (_victor == 0)
            {
                winText.text = "it's a draw";
            }
            else
            {
                winText.text = "player " + (_victor == 1 ? "BLUE" : _victor == 2 ? "RED" : _victor == 3 ? "GREEN" : "ORANGE") + " won";
            }
        }

        if (_victory == 0) _timer -= Time.deltaTime;
        if (_timer <= 0 && ScoreTracker.Instance.Scores.Count > 0 && Timer != 0 && _victory == 0)
        {
            _timer = Timer;
            _victory += Time.deltaTime;
            var maxKills = ScoreTracker.Instance.Scores.Max(score => score.kills);
            var victors = ScoreTracker.Instance.Scores.Where(score => score.kills == maxKills);
            var minDeath = ScoreTracker.Instance.Scores.Min(score => score.deaths);
            victors = victors.Where(score => score.deaths == minDeath);
            var victor = victors.Any() ? victors.SingleOrDefault() : null;
            if (victor != null) _victor = victor.playerId;
            else _victor = -1;

            for (int i = 0; i <= 4; i++)
            {
                var player = ScoreTracker.Instance.Scores.SingleOrDefault(score => score.playerId == i);
                if (player == null) continue;
                player.core.Damage(player.core.CurrentHealth, 0);
                Hit[i] = 0;
            }
        }
        if (_victory > 0)
        {
            _victory += Time.deltaTime;
            if (_victory >= VictoryTIme) { _victory = 0; _victor = -1; CountDown.Clear(); ScoreTracker.Instance.Scores.Clear(); }
        }
        for (int i = 0; i <= 4; i++)
        {
            if (timeText != null)
            {

                if (ScoreTracker.Instance.Scores.Count == 0)
                {
                    timeText.rectTransform.parent.localScale = new Vector2(1/0.11f, 1/0.73f) * (0.8f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.2f));
                    if (i > 0)
                    {
                        if (Input.GetKeyDown("joystick " + i + " button 5")) Timer += 30;
                        if (Input.GetKeyDown("joystick " + i + " button 4")) Timer -= 30;
                    } 
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E)) Timer += 30;
                        if (Input.GetKeyDown(KeyCode.Q)) Timer -= 30;
                    }
                    if (Timer < 0) Timer = 0;

                    timeText.text = "timer   " + (Timer == 0 ? "ENDLESS" : Timer + " seconds");
                    _timer = Timer;
                }
                else if (Timer != 0 && _victory == 0)
                {
                    timeText.rectTransform.parent.localScale = new Vector2(1/0.11f,1/0.73f) * (0.5f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.1f));
                    timeText.text = "timeleft   " + Mathf.FloorToInt(_timer);
                }
                else
                {
                    timeText.rectTransform.parent.localScale = Vector2.Lerp(timeText.rectTransform.parent.localScale, Vector2.zero, 0.2f);
                }
            }

            var container = FindChild("Player " + (i), transform) == null
                ? null
                : FindChild("Player " + (i), transform).GetComponent<RectTransform>();
            var killText = FindChild("Player " + (i) + " Kills", transform) == null
                ? null
                : FindChild("Player " + (i) + " Kills", transform).GetComponent<Text>();
            var deathText = FindChild("Player " + (i) + " Deaths", transform) == null
                ? null
                : FindChild("Player " + (i) + " Deaths", transform).GetComponent<Text>();
            var hpText = FindChild("Player " + (i) + " Health", transform) == null
                ? null
                : FindChild("Player " + (i) + " Health", transform).GetComponent<Text>();
            var overText = FindChild("Player " + (i) + " Health", transform) == null
                ? null
                : FindChild("Player " + (i) + " Over", transform).GetComponent<Text>();
            var join = FindChild("Player " + (i) + " Join", transform) == null
                ? null
                : FindChild("Player " + (i) + " Join", transform).GetComponent<Text>();
            var logo = FindChild("Player " + (i) + " Logo", transform) == null
                ? null
                : FindChild("Player " + (i) + " Logo", transform).GetComponent<Image>();

            if (players.ContainsKey(i))
            {
                if (killText != null)
                {
                    killText.text = players[i].kills.ToString();
                }
                if (deathText != null)
                {
                    deathText.text = players[i].deaths.ToString();
                }
                if (hpText != null)
                {
                    hpText.text = players[i].core != null ? (players[i].core.CurrentHealth / 10).ToString() : "0";
                }
                if (overText != null)
                {
                    overText.rectTransform.localScale = (players.ContainsKey(i) && players[i].core == null && CanSpawn)
                        ? Vector2.one * (0.9f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.1f))
                        : Vector2.zero;
                    if (players.ContainsKey(i) && players[i].core == null && CanSpawn)
                    {
                        if (!CountDown.ContainsKey(i)) CountDown[i] = 10;
                        CountDown[i] -= Time.deltaTime;
                        if (CountDown[i] < 0)
                        {
                            ScoreTracker.Instance.Leave(i);
                        }
                        else
                        {
                            overText.text = "press X to\nrespawn\n" + Mathf.FloorToInt(CountDown[i]);
                        }
                    }
                    else
                    {
                        if (CountDown.ContainsKey(i)) CountDown.Remove(i);
                    }
                }
            }
            if (logo != null)
            {
                if (!originalColors.ContainsKey(i)) originalColors[i] = logo.material.color;
                if (container != null)
                {
                    if (!originalPosis.ContainsKey(i)) originalPosis[i] = container.parent.position;
                    container.parent.position = Vector3.Lerp(container.parent.position,
                        (_victory > 0 && _victor == i) ? new Vector2(Screen.width, Screen.height) / 2 : originalPosis[i], 0.01f);

                    container.localScale = Vector2.Lerp(container.localScale, (_victory > 0 && _victor == i ? 3 : (logo.enabled ? 1 : 0)) * Vector2.one, 0.05f);
                    if (Hit.ContainsKey(i) && Hit[i] > 0)
                    {
                        container.localPosition = Vector2.right * Hit[i] * Mathf.Cos(Time.time * 100) * ShakeAmount;
                        logo.color = Color.Lerp(originalColors[i], Color.black, Hit[i] / 0.5f);
                        Hit[i] -= Time.deltaTime;
                    }
                    else
                    {
                        container.localPosition = Vector2.zero;
                        logo.color = originalColors[i];
                    }
                    if (players.ContainsKey(i) && players[i].core == null && _victor != i)
                    {
                        logo.color = new Color(0.1f, 0.1f, 0.1f);
                    }
                }
                logo.enabled = players.ContainsKey(i);
            }
            if (join != null)
            {
                join.rectTransform.localScale = (!players.ContainsKey(i))
                    ? Vector2.one * (0.8f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.2f))
                    : Vector2.zero;
            }
        }
    }

    private bool ShowTitle()
    {
        return ScoreTracker.Instance.Scores.Count == 0 && CanSpawn;
    }

    private static Transform FindChild(string name, Transform transform)
    {
        var child = transform.FindChild(name);
        if (child != null) return child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = FindChild(name, transform.GetChild(i));
            if (child != null) return child;
        }
        return null;
    }
}
