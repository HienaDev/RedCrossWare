using UnityEngine;
using DG.Tweening;
using TMPro;

public class PopOutAndSlideDown : MonoBehaviour
{
    public Vector3 popScale = new Vector3(1.2f, 1.2f, 1f);
    public float popDuration = 0.5f;
    public float slideDuration = 1f;
    public float slideDistance = 200f;
    public Ease popEase = Ease.OutBack;
    public Ease slideEase = Ease.OutQuad;
    public float delayBetweenAnimations = 0.5f;
    [Range(0f, 1f)] public float colorBrightness = 0.9f; // Controls how bright/light the pastel colors are

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private TextMeshProUGUI textMeshPro;

    [SerializeField] private string prompt = "Banana";

    void Awake()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void AnimatePopAndSlide(string text)
    {
        transform.position = originalPosition;
        transform.localScale = originalScale;
        textMeshPro.text = text;

        if (textMeshPro != null)
        {
            SetPastelColors();
        }

        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(Vector3.Scale(originalScale, popScale), popDuration).SetEase(popEase));
        animationSequence.Append(transform.DOMoveY(originalPosition.y - slideDistance, slideDuration).SetEase(slideEase));
        animationSequence.AppendInterval(delayBetweenAnimations);
    }

    // Method to generate and set pastel colors
    void SetPastelColors()
    {
        textMeshPro.ForceMeshUpdate();
        int characterCount = textMeshPro.textInfo.characterCount;

        if (characterCount > 0)
        {
            for (int i = 0; i < characterCount; i++)
            {
                if (textMeshPro.textInfo.characterInfo[i].isVisible)
                {
                    int materialIndex = textMeshPro.textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textMeshPro.textInfo.characterInfo[i].vertexIndex;

                    // Generate pastel color
                    Color32 pastelColor = GeneratePastelColor();

                    textMeshPro.textInfo.meshInfo[materialIndex].colors32[vertexIndex] = pastelColor;
                    textMeshPro.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 1] = pastelColor;
                    textMeshPro.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 2] = pastelColor;
                    textMeshPro.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 3] = pastelColor;
                }
            }

            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    // Generates a random pastel color
    Color32 GeneratePastelColor()
    {
        // Start with a primary color component (0.5-1.0 range)
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);

        // Make one component dominant (for recognizable colors)
        int dominant = Random.Range(0, 3);
        switch (dominant)
        {
            case 0: r = Random.Range(0.7f, 1f); break; // Red dominant
            case 1: g = Random.Range(0.7f, 1f); break; // Green dominant
            case 2: b = Random.Range(0.7f, 1f); break; // Blue dominant
        }

        // Mix with white to create pastel effect
        float whiteness = Random.Range(0.4f, 0.7f);
        r = Mathf.Lerp(r, 1f, whiteness);
        g = Mathf.Lerp(g, 1f, whiteness);
        b = Mathf.Lerp(b, 1f, whiteness);

        // Apply brightness control
        r = Mathf.Lerp(r, 1f, colorBrightness - 0.7f);
        g = Mathf.Lerp(g, 1f, colorBrightness - 0.7f);
        b = Mathf.Lerp(b, 1f, colorBrightness - 0.7f);

        return new Color32(
            (byte)(r * 255),
            (byte)(g * 255),
            (byte)(b * 255),
            255);
    }
}