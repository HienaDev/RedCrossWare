using UnityEngine;

public class MovingBuildings : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the building moves to the left

    void Update()
    {
        // Move the building to the left
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
}
