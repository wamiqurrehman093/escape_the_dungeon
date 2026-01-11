using UnityEngine;

public class CoinCollectible : CollectibleBase
{
    [Header("Coin Settings")]
    [SerializeField] private float rotationSpeed = 100.0f;
    [SerializeField] private int coinValue = 1;
    protected override void UpdateCollectible()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
    protected override void OnCollected()
    {
        CoinManager.Instance.AddCoins(coinValue);
    }
}
