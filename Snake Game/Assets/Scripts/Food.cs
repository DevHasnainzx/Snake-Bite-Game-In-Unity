using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Serialized fields for customization
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float gridSize = 0.5f; // Must match snake's segmentSpacing

    // Main respawn function
    public void Respawn(List<Transform> snakeSegments)
    {
        Vector2 newPosition;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            // Generate grid-aligned position
            newPosition = new Vector2(
                Mathf.Round(Random.Range(minX, maxX) / gridSize) * gridSize,
                Mathf.Round(Random.Range(minY, maxY) / gridSize) * gridSize
            );
            attempts++;
        }
        while (IsPositionOccupied(newPosition, snakeSegments) && attempts < maxAttempts);

        transform.position = newPosition;
    }

    // Checks if position overlaps with snake
    private bool IsPositionOccupied(Vector2 position, List<Transform> snakeSegments)
    {
        foreach (var segment in snakeSegments)
        {
            if ((Vector2)segment.position == position)
                return true;
        }
        return false;
    }

    // Visual debug (optional)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * gridSize);
    }
}