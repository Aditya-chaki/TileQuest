using DG.Tweening;
using UnityEngine;

public class UITweening : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [field: SerializeField] public bool Active { get; private set; }

    public float fadeSpeed = 0.5f;
    public float popScaleFactor = 1.2f;
    public float popDuration = 0.3f;

    void Start()
    {
        Active = true;
        ToggleState(false);
    }

    public void FadeIn(float speed)
    {
        if (Active) return;

        SoundManager.instance?.PlaySound_BlockMoveFinish();
        canvasGroup?.DOFade(1f, speed).OnComplete(() => ToggleState(true));
    }

    public void FadeOut(float speed)
    {
        if (!Active) return;

        SoundManager.instance?.PlaySound_BlockClick();
        canvasGroup?.DOFade(0f, speed).OnComplete(() => ToggleState(false));
    }

    public void Pop()
    {
        if (!Active) return;

        transform.DOScale(popScaleFactor, popDuration / 2).SetEase(Ease.OutBack)
            .OnComplete(() => transform.DOScale(1f, popDuration / 2).SetEase(Ease.InBack));
    }

    public void Shake() => Shake(0.5f);

    public void Shake(float duration = 0.5f, float strength = 10f, int vibrato = 10)
    {
        SoundManager.instance?.PlaySound_ShowBoard();
        transform.DOShakePosition(duration, strength, vibrato);
    }

    public void Rotate(float duration = 1f, float degrees = 90f)
    {
        if (!Active) return;

        transform.DORotate(new Vector3(0, 0, degrees), duration, RotateMode.FastBeyond360);
    }

    private void ToggleState(bool tru)
    {
        if (Active == tru || canvasGroup == null) return;

        Active = tru;

        if (!Active)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
