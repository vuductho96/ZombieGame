using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject MoneyDrop;
    public ParticleSystem Blood;
    public Animator anim;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public GameObject Health;
    public Slider healthSlider;
    private float damageAmount;

    private void Start()
    {
        Health.SetActive(false);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        MoneyDrop.SetActive(false); // Deactivate the MoneyDrop object at the start
        anim = GetComponent<Animator>();
    }

    public void UpdateHealth(float newHealth)
    {
        currentHealth = newHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
    }

    public void TakeDamageFromPlayer(float damageAmount)
    {
        Health.SetActive(true);
        Blood.Play();
        anim.SetTrigger("EnemyHit");

        currentHealth -= damageAmount;
        UpdateHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        Health.SetActive(false);

        // Detach the MoneyDrop object from the enemy
        MoneyDrop.transform.parent = null;

        // Delay for a certain amount of time before destroying the enemy object
        StartCoroutine(DestroyAfterDelay(1.4f));
        MoneyStay(); // Activate the MoneyDrop object when the enemy dies
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(gameObject);
    }

    public void ApplyKnockBack(Vector3 knockBackDirection)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockBackDirection, ForceMode.Impulse);
        }
    }

    private void MoneyStay()
    {
        MoneyDrop.SetActive(true);
    }
}
