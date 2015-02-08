using UnityEngine;
using System.Collections;

public class PickLevel : MonoBehaviour
{
    public bool First;

    private int _level;

    public void Start()
    {
        if (First)
        {
            _level = Random.Range(1, Application.levelCount);
            Application.LoadLevel(_level);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (ScoreTracker.Instance.Scores.Count > 0) return;

        for (int i = 1; i <= 4; i++)
        {
            var change = false;
            var inputmax = Mathf.Max(Input.GetAxis("Joystick_" + i + "_Left_x"),
                Input.GetAxis("Joystick_" + i + "_Left_y"));
            var inputmin = Mathf.Min(Input.GetAxis("Joystick_" + i + "_Left_x"),
                Input.GetAxis("Joystick_" + i + "_Left_y"));
            if (inputmax >= 0.95f)
            {
                _level++;
                change = true;
            }
            else if (inputmin <= -0.95f)
            {
                _level--;
                change = true;
            }

            while (_level <= 1) _level += Application.levelCount - 1;
            while (_level >= Application.levelCount) _level -= Application.levelCount - 1;

            if (change) { 
                Application.LoadLevel(_level);
                return;
            }
        }
    }
}
