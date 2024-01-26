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
    
    [SerializeField] private Transform _groundChecker;
    
    [SerializeField] private float _groundCheckerRadius; 
    [SerializeField] private LayerMask _groundLayer;
    [FormerlySerializedAs("boxCollider2D")] 
    [Header("Crouch")] 
    [SerializeField] private BoxCollider2D _boxCollider2D; 
    [SerializeField] private Transform _cellChecker; 
    [Range(0, 1)] 
    [SerializeField] private float _crouchModificator;
    [SerializeField] private Rigidbody2D _rigidbody2D; 
    private float _direction; 
    private bool _jump;
        void Update()
        {
            _direction = Input.GetAxisRaw("Horizontal");
            if(Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        }

        private void FixedUpdate()
        {
            bool isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _groundLayer);
            bool canStand = Physics2D.OverlapBox(_cellChecker.position, _boxCollider2D.size, _groundLayer);
            
            if (_jump && isGrounded)
                _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _jump = false;
            
            if (Input.GetAxis("Crouch") > 0 || !canStand)
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
