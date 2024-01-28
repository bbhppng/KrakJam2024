using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _facingRight = true;
    [FormerlySerializedAs("jumpForce")]
    
    [Header("Vertical")]
    
    [SerializeField] private float _jumpForce = 15f;

    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private float _jumpTime;
    private bool isJumping;
    private float _jumpCounter;
    
    [SerializeField] private Transform _groundChecker;
    
    [SerializeField] private float _groundCheckerRadius; 
    [SerializeField] private LayerMask _groundLayer;
    [Header("Crouch")] 
    [SerializeField] private BoxCollider2D _boxCollider2D; 
    [SerializeField] private Transform _cellChecker; 
    [Range(0, 1)] 
    [SerializeField] private float _crouchModificator;
    [SerializeField] private Rigidbody2D _rigidbody2D; 
    public float _direction; 
    private bool _jump;
    [SerializeField] private Animator _animator;
    private Vector2 _vecGravity;
    
        void Update()
        {
            bool canStand = Physics2D.OverlapBox(_cellChecker.position, _boxCollider2D.size, _groundLayer);
            bool isGrounded = Physics2D.OverlapCapsule(_groundChecker.position, new Vector2(0.7371427f, 0.1142798f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
            _direction = Input.GetAxisRaw("Horizontal");
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_direction * 2f,_jumpForce);
                isJumping = true;
                _jumpCounter = 0f;
            }
            if (_rigidbody2D.velocity.y > 0 && isJumping)
            {
                _jumpCounter += Time.deltaTime;
                if (_jumpCounter > _jumpTime) 
                    isJumping = false;
                float _t = _jumpCounter / _jumpTime;
                float _currentJumpM = _jumpMultiplier;
                if (_t > 0.5f)
                {
                    _currentJumpM = _jumpMultiplier * (1 - _t);
                }

                _rigidbody2D.velocity += _vecGravity * _currentJumpM * Time.deltaTime;
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
                _jumpCounter = 0;
                if (_rigidbody2D.velocity.y > 0)
                {
                    _rigidbody2D.velocity = new Vector2(_direction * 2f, _rigidbody2D.velocity.y * 0.6f);
                }
                _animator.SetBool("isJumping", false);
            }

            if (_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.velocity -= _vecGravity * _fallMultiplier * Time.deltaTime;
            }
            if (Input.GetButtonDown("Crouch") || !canStand)
                _boxCollider2D.enabled = false;
            else
                _boxCollider2D.enabled = true;
            
            if (isGrounded)
            {
                float speedModificator = 1;
                if (!canStand)
                    speedModificator = _crouchModificator;
                _rigidbody2D.velocity = new Vector2(_direction * _speed * speedModificator, _rigidbody2D.velocity.y);
                SetDirection();
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(_direction * _speed, _rigidbody2D.velocity.y);
                SetDirection();
            }

            if (isJumping == true)
            {
                _animator.SetBool("isJumping", true);
            }

            if (_direction > 0f)
                _animator.SetBool("isRunning", true);
            else if(_direction < 0f)
                _animator.SetBool("isRunning", true);
            else
                _animator.SetBool("isRunning", false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
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
}
