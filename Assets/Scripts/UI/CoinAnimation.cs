using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinAnimationController : MonoBehaviour
{
    public GameObject coinPrefab;     // Reference to the coin prefab
    public Transform startPoint;      // Where coins originate (e.g., "Level Complete" text)
    public Transform endPoint;        // Destination (e.g., "GET" button)
    public int coinCount = 10;        // Number of coins to animate
    public float spawnDelay = 0.1f;   // Delay between coin spawns
    public float scatterRadius = 2f;  // Radius for scattering coins
    public float scatterDuration = 0.5f; // Duration of scatter animation

    void Start()
    {
        
    }

    public void AnimateCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            // Instantiate coin at start point
            GameObject coin = Instantiate(coinPrefab, startPoint.position, Quaternion.identity, transform);

            // Calculate random scatter position
            Vector3 scatterPosition = startPoint.position + (Vector3)Random.insideUnitCircle * scatterRadius;

            // Calculate delay for this coin
            float delay = i * spawnDelay;

            // Create sequence for scatter then move to endpoint
            Sequence coinSequence = DOTween.Sequence();
            
            // First move to scatter position
            coinSequence.Append(coin.transform.DOMove(scatterPosition, scatterDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad));
            
            // Then move to endpoint
            coinSequence.Append(coin.transform.DOMove(endPoint.position, 1f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    // Destroy coin after reaching the destination
                    Destroy(coin);
                }));

            // Play the sequence
            coinSequence.Play();
        }
    }
}