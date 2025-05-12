using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float stamina = 100f;

    public float Health { get => health; set => health = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    internal void Die()
    {
        print("Player has died.");
        Player.Instance.gameObject.SetActive(false);
    }
}
