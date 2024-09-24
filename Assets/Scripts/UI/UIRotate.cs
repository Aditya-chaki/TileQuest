using UnityEngine;
using DG.Tweening;

public class UIRotate : MonoBehaviour
{
    public float speed = 1f;

    void Start()
    {
        RotateUI();
    }

    void RotateUI()
    {
        transform.DORotate(new Vector3(0, 0, 360), speed, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
}
