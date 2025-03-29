using UnityEngine;
using DG.Tweening;

public class HopAnimation : MonoBehaviour
{
    public float hopHeight = 1f;        // How high the character hops (scaled up in Y)
    public float hopDistance = 1f;      // How far left and right the character moves (rotation)
    public float hopDuration = 0.5f;    // Duration of each hop
    public float scaleFactor = 1.1f;    // Scale increase during hop
    public float rotationAngle = 10f;   // Rotation angle when hopping

    private Vector3 originalScale;
    private Vector3 originalPosition;
    public bool hopping = false;
    private Sequence hopSequence;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;

    }

    public void StopHopping()
    {
        if (hopSequence != null)
        {
            hopping = false;
            hopSequence.Kill();
        }
    }

    public void StartHopping()
    {
        if (hopping)
            return;

        hopping = true;
        hopSequence = DOTween.Sequence();

        // Hop to the right: Rotate right, scale up, and then scale back down
        hopSequence.Append(transform.DORotate(new Vector3(0, 0, rotationAngle), hopDuration / 2).SetEase(Ease.OutQuad));
        hopSequence.Join(transform.DOScale(originalScale * scaleFactor, hopDuration / 2).SetEase(Ease.OutQuad));

        hopSequence.Append(transform.DORotate(Vector3.zero, hopDuration).SetEase(Ease.InOutQuad)); // Rotate back to normal
        hopSequence.Append(transform.DOScale(originalScale, hopDuration).SetEase(Ease.InOutQuad)); // Scale back down

        // Hop to the left: Rotate left, scale up, and then scale back down
        hopSequence.Append(transform.DORotate(new Vector3(0, 0, -rotationAngle), hopDuration / 2).SetEase(Ease.OutQuad));
        hopSequence.Join(transform.DOScale(originalScale * scaleFactor, hopDuration / 2).SetEase(Ease.OutQuad));

        hopSequence.Append(transform.DORotate(Vector3.zero, hopDuration).SetEase(Ease.InOutQuad)); // Rotate back to normal
        hopSequence.Append(transform.DOScale(originalScale, hopDuration).SetEase(Ease.InOutQuad)); // Scale back down

        // Repeat indefinitely
        hopSequence.SetLoops(-1);
    }
}
