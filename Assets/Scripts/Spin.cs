using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public float GrowAmount;
    public float TurnSpeed;
    private Vector3 _scale;

    public void Start()
    {
        _scale = transform.localScale;
    }

    public void Update()
    {
        transform.localScale = _scale + Vector3.one * Mathf.Abs(Mathf.Cos(Time.time * 2) * GrowAmount);
        transform.Rotate(Vector3.forward, TurnSpeed*Time.deltaTime);
    }
}
