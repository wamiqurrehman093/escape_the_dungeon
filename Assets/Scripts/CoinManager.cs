using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private UIManager uiManager;
    private int totalCoins = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GetUIManagerReference();
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
        if (uiManager != null)
        {
            uiManager.UpdateCoinsText(totalCoins);
        }
    }
    private void GetUIManagerReference()
    {
        GameObject uiManagerObject = GameObject.FindWithTag("UIManager");
        Instance.uiManager = uiManagerObject.GetComponent<UIManager>();
    }
    public void Reset()
    {
        totalCoins = 0;
    }
}
