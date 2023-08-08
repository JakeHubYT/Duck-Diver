using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.DealDamage(damageAmount);
        }
    }
}
