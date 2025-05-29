using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 10f;
    [SerializeField] private float damage = 10f;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }

    // this is the method that will be called when the enemy takes damage
    // it will reduce the health of the enemy and check if it is dead
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    // this method will be called when the enemy dies
    // it will destroy the enemy game object after a delay
    private void Die()
    {
        float delay = 0.5f; 
        Destroy(this, delay);
    }
}
