using UnityEngine;
using System.Collections;

public class PickLevel : MonoBehaviour
{
    public bool First;
    public float CreditsTimer = 5;
 
    private int _level;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (CreditsTimer > 0)
        {
            CreditsTimer -= Time.deltaTime;
            if (CreditsTimer <= 0)
            {
                if (First)
                {
                    _level = Random.Range(1, Application.levelCount);
                    Application.LoadLevel(_level);
                }
            }
            return;
        }

        if (ScoreTracker.Instance.Scores.Count > 0) return;

        for (int i = 0; i <= 4; i++)
        {
            var change = false;
            var inputmax = i > 0 
                ? Mathf.Max(Input.GetAxis("Joystick_" + i + "_Left_x"),
                    Input.GetAxis("Joystick_" + i + "_Left_y"))
                : Mathf.Max((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
                    (Input.GetKey(KeyCode.S) ? 1 : 0) - (Input.GetKey(KeyCode.W) ? 1 : 0));
            var inputmin = i > 0
                ? Mathf.Min(Input.GetAxis("Joystick_" + i + "_Left_x"),
                    Input.GetAxis("Joystick_" + i + "_Left_y"))
                : Mathf.Min((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
                    (Input.GetKey(KeyCode.S) ? 1 : 0) - (Input.GetKey(KeyCode.W) ? 1 : 0));
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
