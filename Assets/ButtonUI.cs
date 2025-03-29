using UnityEngine;
using DG.Tweening;

public class ButtonUI : MonoBehaviour
{
    public float scaleUpSize = 1.2f;  // Scale size when hovering
    public float scaleDownSize = 1f;  // Normal size
    public float moveDistance = 10f;  // Distance moved to the right
    public float exitDistance = 20f;  // Distance moved to the left for exit
    public float animationTime = 0.2f; // Animation duration
    public Ease easeType = Ease.OutBack; // Easing effect

    private Vector3 originalPosition;
    private float originalScale;

    private bool exiting = false;

    void Start()
    {
        originalPosition = transform.position; // Save initial position
        originalScale = transform.localScale.x;
    }

    // Scale up (hover effect)
    public void ScaleUp()
    {
        transform.DOScale(originalScale * scaleUpSize, animationTime).SetEase(Ease.OutQuad);
    }

    // Scale down (return to normal size)
    public void ScaleDown()
    {
        transform.DOScale(originalScale * scaleDownSize, animationTime).SetEase(Ease.InQuad);
    }

    // Move slightly to the right
    public void MoveRight()
    {
        if (exiting) return;
        transform.DOMoveX(originalPosition.x + moveDistance, animationTime).SetEase(easeType);
    }

    // Move back to the original position
    public void ResetPosition()
    {
        if (exiting) return;
        transform.DOMove(originalPosition, animationTime).SetEase(easeType);
    }

    // Click animation (scales down slightly then back up)
    public void ClickEffect()
    {
        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.InQuad));
        clickSequence.Append(transform.DOScale(originalScale * scaleDownSize, 0.15f).SetEase(Ease.OutBack));
    }

    // **Exit animation (moves far left)**
    public void ExitLeft()
    {
        exiting = true;
        transform.DOMoveX(originalPosition.x - exitDistance, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => exiting = false); // Deactivate after animation
    }

    // **Exit the game (works in both Editor and Build)**
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
            Application.Quit(); // Quit game in build
#endif
    }
}
