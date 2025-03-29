using System.Collections;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private float buttonPressTime = 1f; // Adjust as needed
    [SerializeField] private Sprite buttonPressedSprite; // Assign in the inspector
    [SerializeField] private Sprite buttonNormalSprite; // Assign in the inspector
    private SpriteRenderer spriteRenderer;

    private bool pressed = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if(pressed)
        {
            return;
        }

        StartCoroutine(PressButton());

        Debug.Log("Button interacted with!");
        // Add your door opening logic here
    }

    private IEnumerator PressButton()
    {
        pressed = true;
        spriteRenderer.sprite = buttonPressedSprite;
        yield return new WaitForSeconds(buttonPressTime);
        spriteRenderer.sprite = buttonNormalSprite;
        pressed = false;
    }
}