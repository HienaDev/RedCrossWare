using System.Collections;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string buttonName = "Button"; // Assign in the inspector

    [SerializeField] private float buttonPressTime = 1f; // Adjust as needed
    [SerializeField] private Sprite buttonPressedSprite; // Assign in the inspector
    [SerializeField] private Sprite buttonNormalSprite; // Assign in the inspector
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] private SpriteRenderer iconSR;

    private bool pressed = false;

    private IMicroGame gameScript;

    
    public Collider2D colliderInteract;
    private void Awake()
    {
        colliderInteract = GetComponent<Collider2D>();

        colliderInteract.enabled = false;
    }

    public void Initialize(Sprite normal, string answer, IMicroGame gameScript)
    {
        iconSR.sprite = normal;
        buttonName = answer;
        this.gameScript = gameScript;

  
    }

    public void Interact()
    {
        if(pressed)
        {
            return;
        }

        StartCoroutine(PressButton());

        gameScript.CheckGame(buttonName);
        Debug.Log("Button interacted with!");
        // Add your door opening logic here
    }

    private IEnumerator PressButton()
    {
        pressed = true;
        spriteRenderer.sprite = buttonPressedSprite;

        Debug.Log((gameScript).TimeLimit);
        yield return new WaitForSeconds(buttonPressTime);
        //spriteRenderer.sprite = buttonNormalSprite;
        //pressed = false;
    }

    public void ResetInteractable()
    {
        spriteRenderer.sprite = buttonNormalSprite;
        colliderInteract.enabled = false;
        pressed = false;
    }
}