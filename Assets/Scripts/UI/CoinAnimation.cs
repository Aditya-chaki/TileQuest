using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinAnimationController : MonoBehaviour
{
    public GameObject coinPrefab; // Reference to the coin prefab
    public Transform startPoint; // Where coins originate (e.g., "Level Complete" text)
    public Transform endPoint;   // Destination (e.g., "GET" button)
    public int coinCount = 10;   // Number of coins to animate
    public float spawnDelay = 0.1f; // Delay between coin spawns

    void Start()
    {
        
    }

    public void AnimateCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, startPoint.position, Quaternion.identity, transform);

            // Wait and animate each coin
            float delay = i * spawnDelay;
            coin.transform.DOMove(endPoint.position, 1f).SetDelay(delay).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    // Destroy coin after reaching the destination
                    Destroy(coin);
                });
        }
    }
}
