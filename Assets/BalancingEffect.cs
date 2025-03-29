using UnityEngine;
using DG.Tweening;

public class BalancingEffect : MonoBehaviour
{
    public float tiltAngle = 10f;   // Maximum tilt angle (left & right)
    public float duration = 1.5f;   // Time to complete one left-right cycle
    public float initialOffset = 5f; // Offset angle to start from

    void Start()
    {
        // Apply the initial offset before starting the sequence
        transform.rotation = Quaternion.Euler(0, 0, initialOffset);

        StartBalancing();
    }

    void StartBalancing()
    {
        Sequence balanceSequence = DOTween.Sequence();

        balanceSequence.Append(transform.DORotate(new Vector3(0, 0, tiltAngle), duration)
            .SetEase(Ease.InOutSine));

        balanceSequence.Append(transform.DORotate(new Vector3(0, 0, -tiltAngle), duration)
            .SetEase(Ease.InOutSine));

        balanceSequence.SetLoops(-1, LoopType.Yoyo); // Infinite back-and-forth
    }
}
