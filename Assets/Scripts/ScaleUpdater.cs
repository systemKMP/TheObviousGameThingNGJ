using UnityEngine;
using System.Collections;

public class ScaleUpdater : MonoBehaviour {

    public float size = 1.0f;

    void Update()
    {
        if (Camera.current != null)
        {
            transform.localScale = Vector3.one * size * Camera.current.orthographicSize;
        }
    }
}
