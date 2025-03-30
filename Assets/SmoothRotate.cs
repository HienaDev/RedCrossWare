using UnityEngine;
using DG.Tweening;

public class SmoothRotate : MonoBehaviour
{
    public float rotationAngle = 10f;  // Max rotation in degrees
    public float duration = 1.5f;      // Time to complete one rotation cycle

    void Start()
    {
        StartRotation();
    }

    void StartRotation()
    {
        // Rotates back and forth around the Z-axis smoothly
        transform.DORotate(new Vector3(0, 0, rotationAngle), duration)
            .SetEase(Ease.InOutSine) // Smooth easing
            .SetLoops(-1, LoopType.Yoyo); // Infinite back-and-forth
    }
}
