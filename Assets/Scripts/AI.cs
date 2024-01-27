using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AI : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _facingRight = true;
    [SerializeField] private Transform _target;
    private Vector3 _targetPosition;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private float _direction;
    [SerializeField] private float _jumpForce = 350f;

    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        _targetPosition = new Vector3(_target.position.x, _rigidbody2D.position.y, 0);
        _rigidbody2D.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        SetDirection();
    }
    
    private void SetDirection()
    {
        if (_facingRight && _direction < 0)
        {
            Flip();
        }
        else if (!_facingRight && _direction > 0)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpForce);
    }
}
