using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    [SerializeField] private TMPro.TextMeshProUGUI coinText;
    private int totalCoins = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
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
