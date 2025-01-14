using UnityEngine;
using DG.Tweening;

public class LosingCoinEffect : MonoBehaviour
{
    public GameObject coinPrefab;  // Reference to the coin prefab
    public Transform startPoint;  // Where coins originate
    public int coinCount = 10;    // Number of coins to scatter
    public float scatterRadius = 2f; // Maximum distance coins will scatter
    public float scatterDuration = 1f; // Time it takes for coins to scatter
    public float fadeDuration = 0.5f;  // Time it takes for coins to fade out

    private void Start() {
        ScatterCoins();
    }
    
    public void ScatterCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            // Create a coin at the start position
            GameObject coin = Instantiate(coinPrefab, startPoint.position, Quaternion.identity, transform);

            // Generate a random direction within a circle
            Vector3 randomDirection = Random.insideUnitCircle.normalized * scatterRadius;
            Vector3 targetPosition = startPoint.position + randomDirection;

            // Animate the coin's movement
            coin.transform.DOMove(targetPosition, scatterDuration).SetEase(Ease.OutQuad);

            // Add random rotation on the Z-axis
            coin.transform.DORotate(new Vector3(0, 0, Random.Range(0f, 360f)), scatterDuration, RotateMode.Fast);

            // Start fade-out after the movement animation
            SpriteRenderer spriteRenderer = coin.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.DOFade(0, fadeDuration).SetDelay(scatterDuration).OnComplete(() =>
                {
                    // Destroy the coin after fading out
                    Destroy(coin);
                });
            }
            else
            {
                // Destroy the coin directly if no SpriteRenderer is found
                Destroy(coin, scatterDuration + fadeDuration);
            }
        }
    }
}
