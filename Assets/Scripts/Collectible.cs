using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour
{
    [Header("Base Collectible Settings")]
    [SerializeField] protected float collectionRadius = 1.5f;
    [SerializeField] protected GameObject collectionEffect;
    [SerializeField] protected AudioClip collectionSound;
    protected bool isCollected = false;
    protected Transform playerTransform;
    void Update()
    {
        if (isCollected) return;
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < collectionRadius)
        {
            Collect();
        }
        UpdateCollectible();
    }
    protected virtual void UpdateCollectible()
    {

    }
    protected virtual void Collect()
    {
        isCollected = true;
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        }
        OnCollected();
        Destroy(gameObject);
    }
    protected abstract void OnCollected();
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
