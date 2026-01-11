using UnityEngine;

public class CoinCollectible : CollectibleBase
{
    [Header("Coin Settings")]
    [SerializeField] private float rotationSpeed = 100.0f;
    protected override void UpdateCollectible()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
    protected override void OnCollected()
    {
        Debug.Log($"Coin collected");
    }
}
