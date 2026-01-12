using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI coinText;
    private int totalCoins = 0;
    void Start()
    {
        UpdateCoinText();
    }
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateCoinText();
    }
    public int GetCoins()
    {
        return totalCoins;
    }
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {totalCoins}";
        }
    }
}
