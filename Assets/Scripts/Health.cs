using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float startHealth = 100f;
    public float currentHealth;
    public bool invincible = false;
    public DeathAction deathAction;
    public float deathDelay = 0f;
    public GameObject[] spawnObjectsOnDeath;
    public GameObject[] destroyObjectsOnDeath;
    public HurtEffect hurtEffect;

    private bool isDead = false;

    // Event for OnDeath
    public event Action OnDeath;

    private void Start()
    {
        currentHealth = startHealth;
    }

    private void Update()
    {
        if (isDead || invincible)
            return;

        // Example: You can implement damage logic here, like OnTriggerEnter, etc.
        // For simplicity, I won't include the damage function in this outline.
    }

    public void TakeDamage(float damageAmount)
    {
        if (invincible || isDead)
            return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        InvokeDeathAction();
        SpawnObjectsOnDeath();
        DestroyObjectsOnDeath();

        // Invoke the OnDeath event
        OnDeath?.Invoke();

        Destroy(gameObject, deathDelay);
    }

    public void DealDamage(float damageAmount)
    {
        if (invincible || isDead)
            return;

        if(hurtEffect!= null)
        hurtEffect.InitiateHurtEffect();


        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Health =" + currentHealth);
    }

    private void InvokeDeathAction()
    {
        switch (deathAction)
        {
            case DeathAction.None:
                // Do nothing
                break;
            case DeathAction.Delay:
                // Implement any specific logic you want to delay the death
                // Example: StartCoroutine(DelayDeathCoroutine());
                break;
                // Add more death actions here if needed
        }
    }

    private void SpawnObjectsOnDeath()
    {
        foreach (GameObject spawnObject in spawnObjectsOnDeath)
        {
            Instantiate(spawnObject, transform.position, transform.rotation);
        }
    }

    private void DestroyObjectsOnDeath()
    {
        foreach (GameObject destroyObject in destroyObjectsOnDeath)
        {
            Destroy(destroyObject);
        }
    }

    // Function to reset health back to its starting value
    public void ResetHealth()
    {
        currentHealth = startHealth;
        isDead = false;
    }
}

public enum DeathAction
{
    None,
    Delay,
    // Add more death actions here if needed
}
