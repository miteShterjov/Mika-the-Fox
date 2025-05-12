using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 10f;
    [SerializeField] private float damage = 10f;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        float delay = 0.5f; 
        Destroy(this, delay);
    }
}
