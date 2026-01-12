using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private GameObject damageEffect;
    private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        if (damageEffect != null)
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed for {amount}. Health: {currentHealth}/{maxHealth}");
    }
    private void Die()
    {
        Debug.Log("Player died!");
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
}