using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    public PlayerMovement Mover;
    public PlayerController Player;

    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Update()
    {
        Player = GetComponent<PlayerController>();
        _animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        _animator.SetFloat("VerticalSpeed", rigidbody2D.velocity.y);
        _animator.SetBool("Grounded", Mover.IsGrounded());
        if(Player != null)_animator.SetFloat("Player", Player.Index-1);
    }
}
