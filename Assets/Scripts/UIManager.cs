using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    void Start()
    {
        gameOverPanel.SetActive(false);
    }
    public void OnPlayAgainClicked()
    {
        GameManager.Instance.PlayAgain();
    }
    public void UpdateCoinsText(int coins)
    {
        coinText.text = $"{coins}";
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void UpdateHealth(float health)
    {
        healthText.text = $"{health}";
        healthSlider.value = health;
    }
    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
    public void SetCurrentHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
