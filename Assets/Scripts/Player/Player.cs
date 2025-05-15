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
    [SerializeField] private int gemsCollected = 0; // Rate at which stamina regenerates

    public float Health { get => health; set => health = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public Vector3 RespawnPoint { get; internal set; }
    public int GemsCollected { get => gemsCollected; set => gemsCollected = value; }

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
}
