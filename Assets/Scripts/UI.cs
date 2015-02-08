using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public float BlinkSpeed = 1.5f;
    public float LogoSize = 115;
    public float ShakeAmount = 50;

    public Dictionary<int, float> Hit = new Dictionary<int, float>();
    public Dictionary<int, float> CountDown = new Dictionary<int, float>(); 

    public static UI Instance;

    private Dictionary<int, Color> originalColors = new Dictionary<int, Color>();

    public void Start()
    {
        Instance = this;
    }

    public void Update()
    {
        var players = new Dictionary<int, PlayerScore>();

        foreach (var score in ScoreTracker.Instance.Scores)
        {
            players[score.playerId] = score;
        }

        for (int i = 0; i < 6; i++)
        {
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
            var title = FindChild("Title", transform) == null
                ? null
                : FindChild("Title", transform).GetComponent<Image>();
            var hint = FindChild("Hint", transform) == null
                ? null
                : FindChild("Hint", transform).GetComponent<Text>();

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
                    overText.rectTransform.localScale = (players.ContainsKey(i) && players[i].core == null)
                        ? Vector2.one * (0.9f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.1f))
                        : Vector2.zero;
                    if (players.ContainsKey(i) && players[i].core == null)
                    {
                        if (!CountDown.ContainsKey(i)) CountDown[i] = 13;
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
                    container.localScale = Vector2.Lerp(container.localScale, (logo.enabled ? 1 : 0) * Vector2.one, 0.1f);
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
                    if (players.ContainsKey(i) && players[i].core == null)
                    {
                        logo.color = new Color(0.1f,0.1f,0.1f);
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
            if (title != null)
            {
                if (hint != null)
                {
                    hint.rectTransform.localScale = Vector2.one*(0.8f + Mathf.Abs(Mathf.Cos(Time.time*2)*0.2f));
                }
                title.rectTransform.localScale = Vector3.Lerp(title.rectTransform.localScale,
                    ShowTitle() ? Vector3.one * (0.9f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.1f)) : Vector3.zero, 0.02f); // TODO pretty transition
            }
        }
    }

    private bool ShowTitle()
    {
        return ScoreTracker.Instance.Scores.Count == 0;
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
