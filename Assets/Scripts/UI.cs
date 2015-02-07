﻿using System.Collections.Generic;
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
                        logo.color = Color.Lerp(originalColors[i], Color.black, Hit[i]/0.5f);
                        Debug.Log(logo.material.color);
                        Hit[i] -= Time.deltaTime;
                    }
                    else
                    {
                        container.localPosition = Vector2.zero;
                        logo.material.color = originalColors[i];
                    }
                }
                logo.enabled = players.ContainsKey(i);
            }
            if (join != null)
            {
                join.rectTransform.localScale = (!players.ContainsKey(i) || players[i].core == null)
                    ? Vector2.one * (0.8f + Mathf.Abs(Mathf.Cos(Time.time * 2) * 0.2f))
                    : Vector2.zero;
            }
        }
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
