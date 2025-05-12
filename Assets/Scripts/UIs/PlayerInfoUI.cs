using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    void Update()
    {
        if (Player.Instance != null)
        {
            healthSlider.maxValue = Player.Instance.MaxHealth;
            healthSlider.value = Player.Instance.Health;
            staminaSlider.maxValue = Player.Instance.MaxStamina;
            staminaSlider.value = Player.Instance.Stamina;
        }
        else
        {
            Debug.LogWarning("Player instance is null. Make sure the Player script is attached to a GameObject in the scene.");
        }
    }
}
