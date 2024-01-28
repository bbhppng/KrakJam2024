using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image _healthbar;
    [SerializeField] private int _maxHealth;
    private int _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _healthbar.fillAmount = _health / 100f;
        if (_health <= 0)
        {
            Die();
            SceneManager.LoadSceneAsync(0);
        }
        
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
