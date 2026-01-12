using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameManager gameManager;
    private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
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
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }
    private void Die()
    {
        Debug.Log("Player died!");
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
            gameObject.SetActive(false);
        }
        if (animator != null)
        {
            animator.SetTrigger("Die");
            gameManager.GameOver();
        }
    }
}