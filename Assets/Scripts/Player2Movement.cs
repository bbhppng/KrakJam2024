using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    [Header("Horizontal")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _facingRight = true;
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
    [SerializeField] private Rigidbody2D _rigidbody2D; 
    public float _direction; 
    private bool _jump;
    [SerializeField] private Animator _animator;
    private Vector2 _vecGravity;
    
        void Update()
        {
            bool isGrounded = Physics2D.OverlapCapsule(_groundChecker.position, new Vector2(0.7371427f, 0.1142798f), CapsuleDirection2D.Horizontal, 0, _groundLayer);
            _direction = Input.GetAxisRaw("Horizontal2");
            if(Input.GetButtonDown("Jump2") && isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_direction * 2f,_jumpForce);
                isJumping = true;
                _jumpCounter = 0f;
            }
            CalculateGravity(); 

            if (Input.GetButtonUp("Jump2"))
            {
                EndJump();
            }

            if (_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.velocity -= _vecGravity * _fallMultiplier * Time.deltaTime;
            }
           
            
            if (isGrounded)
            {
                float speedModificator = 1;
                _rigidbody2D.velocity = new Vector2(_direction * _speed * speedModificator, _rigidbody2D.velocity.y);
                SetDirection();
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(_direction * _speed, _rigidbody2D.velocity.y);
                SetDirection();
            }

            UpdateAnimator();
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

        private void EndJump()
        {
            isJumping = false;
            _jumpCounter = 0;
            if (_rigidbody2D.velocity.y > 0)
            {
                _rigidbody2D.velocity = new Vector2(_direction * 2f, _rigidbody2D.velocity.y * 0.6f);
            }
            _animator.SetBool("isJumping", false);
        }
        
        private void CalculateGravity()
        {
            if (_rigidbody2D.velocity.y > 0 && isJumping)
            {
                _jumpCounter += Time.deltaTime;
                if (_jumpCounter > _jumpTime)
                    isJumping = false;
                float t = _jumpCounter / _jumpTime;
                float currentJumpMultiplier = (t > 0.5f) ? _jumpMultiplier * (1 - t) : _jumpMultiplier;
                _rigidbody2D.velocity += _vecGravity * currentJumpMultiplier * Time.deltaTime;
            }
        }

        private void UpdateAnimator()
        {
            if (isJumping)
            {
                _animator.SetBool("isJumping", true);
            }

            if (_direction != 0f)
                _animator.SetBool("isRunning", true);
            else
                _animator.SetBool("isRunning", false);
        }
}
