using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Update()
    {
        _animator.SetFloat("Speed", rigidbody2D.velocity.magnitude);
    }
}
