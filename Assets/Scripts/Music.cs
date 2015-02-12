using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    private static Music _instance;

    public void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            transform.parent = null;
        }
    }
}
