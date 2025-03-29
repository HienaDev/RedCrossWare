using UnityEngine;
using System.Collections;

// Then create the interaction handler script
public class MouseInteraction : MonoBehaviour
{

    [SerializeField] private float maxInteractionDistance = 100f; // Adjust as needed
    [SerializeField] private LayerMask interactableLayers = ~0; // Default to all layers

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            CheckForInteractable();
        }
    }

    private void CheckForInteractable()
    {
        // Convert mouse position to world point (for 2D)
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Perform the 2D raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, maxInteractionDistance, interactableLayers);

        if (hit.collider != null)
        {
            // Try to get an IInteractable component from the hit object
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            Debug.Log(interactable);
            if (interactable != null)
            {
                // If the object has the interface, call its Interact method
                interactable.Interact();
            }
        }
    }
}