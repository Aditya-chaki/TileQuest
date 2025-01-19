using UnityEngine;
using DG.Tweening;

public class LosingCoinEffect : MonoBehaviour
{
    public GameObject coinPrefab;  // Reference to the coin prefab
    public Transform startPoint;  // Where coins originate
    public int coinCount = 10;    // Number of coins to fall
    public float fallHeight = 5f; // Maximum height from where coins fall
    public float fallDuration = 1f; // Time it takes for coins to fall
    public float fadeDuration = 0.5f;  // Time it takes for coins to fade out

    private void Start()
    {
        // ScatterCoins();
    }

    public void ScatterCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            // Create a coin at a randomized height above the start position
            Vector3 spawnPosition = startPoint.position + new Vector3(
                Random.Range(-1f, 1f), // Randomize X position slightly for spread
                Random.Range(0f, fallHeight), // Randomize height within fallHeight
                0
            );

            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, transform);

            // Target position is directly below the spawn position
            Vector3 targetPosition = new Vector3(spawnPosition.x, startPoint.position.y, spawnPosition.z);

            // Animate the coin's falling motion
            coin.transform.DOMove(targetPosition, fallDuration).SetEase(Ease.InQuad);

            // Add random rotation on the Z-axis during the fall
            coin.transform.DORotate(new Vector3(0, 0, Random.Range(0f, 360f)), fallDuration, RotateMode.Fast);

            // Start fade-out after the fall animation
            SpriteRenderer spriteRenderer = coin.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.DOFade(0, fadeDuration).SetDelay(fallDuration).OnComplete(() =>
                {
                    // Destroy the coin after fading out
                    Destroy(coin);
                });
            }
            else
            {
                // Destroy the coin directly if no SpriteRenderer is found
                Destroy(coin, fallDuration + fadeDuration);
            }
        }
    }
}
