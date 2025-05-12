using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DoDamageToPlayer(other.gameObject);
        }
    }

    private void DoDamageToPlayer(GameObject other)
    {
        Player.Instance.Health -= other.GetComponent<Enemy>().Damage;
        
        if (Player.Instance.Health <= 0) Player.Instance.Die();
    }
}
