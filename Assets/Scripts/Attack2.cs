using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{
    [SerializeField] private Transform _attackChecker;
    [SerializeField] private float _attackCheckerRange;
    [SerializeField] private LayerMask _enemyLayer;
    private bool isHardAttacking = false;
    private bool isFistAttacking = false;
    public float _cooldownTime; 
    public float _FistcooldownTime; 
    private float _nextAttackTime = 0.0f;
    private float _nextFistAttackTime = 0.0f;
    [SerializeField] private Animator _animator;
    
    void Update()
    {
        if (Input.GetButtonDown("HardAttack2") && !isHardAttacking && Time.time > _nextAttackTime)
        {
            _cooldownTime = 10f;
            HardAttack();
            isHardAttacking = true;
            _nextAttackTime = Time.time + _cooldownTime;
            
        }
        HardAttackAnimation();
      

        if (Input.GetButtonDown("FistAttack2") && !isFistAttacking && Time.time > _nextFistAttackTime)
        {
            _FistcooldownTime = 2f;
            FistAttack();
            isFistAttacking = true;
            _nextFistAttackTime = Time.time + _FistcooldownTime;
        }
        
        FistAttackAnimation();
        
    }

    

    private void HardAttack()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackChecker.position, _attackCheckerRange, _enemyLayer);
        
        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Health>().TakeDamage(25);
        }
    }
    
    private void FistAttack()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackChecker.position, _attackCheckerRange, _enemyLayer);
        
        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Health>().TakeDamage(5);
        }
    }

    private void HardAttackAnimation()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("kuoka_hardattack") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isHardAttacking = false;
        }
        _animator.SetBool("isAttacking", isHardAttacking);
    }

    private void FistAttackAnimation()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("kuoka_fistattack") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isFistAttacking = false;
        }
        _animator.SetBool("isFistAttacking", isFistAttacking);
    }
    
    

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackChecker.position, _attackCheckerRange);
    }
}
