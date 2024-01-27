using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDefaul : MonoBehaviour
{
    [SerializeField] private Transform _attackChecker;
    [SerializeField] private float _attackCheckerRange;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _coolDown;
    private bool isAttacking = false;
    public float _cooldownTime = 1.5f; 
    private float _nextAttackTime = 0.0f;
    [SerializeField] private Animator _animator;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking && Time.time > _nextAttackTime)
        {
            PlayerAttack();
            isAttacking = true;
            _nextAttackTime = Time.time + _cooldownTime;
            
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("kuoka_hardattack") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isAttacking = false;
        }
        

        _animator.SetBool("isAttacking", isAttacking);
        
    }

    

    private void PlayerAttack()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackChecker.position, _attackCheckerRange, _enemyLayer);
        
        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Enemy>().TakeDamage(4);
        }
    }
    
    

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackChecker.position, _attackCheckerRange);
    }
}
