using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // Standard widescreen (adjust if needed)

    void Start()
    {
        // Calculate current screen ratio
        float currentAspect = (float)Screen.width / Screen.height;
        float scaleRatio = currentAspect / targetAspect;

        // Adjust camera viewport
        Camera cam = GetComponent<Camera>();
        if (scaleRatio < 1f) // Tall screen (e.g., 9:16)
        {
            cam.rect = new Rect(0, (1f - scaleRatio) / 2f, 1f, scaleRatio);
        }
        else // Wide screen (e.g., 18:9)
        {
            float scaleWidth = 1f / scaleRatio;
            cam.rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
        }

        // Position walls dynamically
        PositionWalls();
    }

    void PositionWalls()
    {
        float viewWidth = Camera.main.orthographicSize * targetAspect;
        float viewHeight = Camera.main.orthographicSize;

        // Adjust these names to match your wall objects
        GameObject.Find("LeftWall").transform.position = new Vector3(-viewWidth, 0, 0);
        GameObject.Find("RightWall").transform.position = new Vector3(viewWidth, 0, 0);
        GameObject.Find("TopWall").transform.position = new Vector3(0, viewHeight, 0);
        GameObject.Find("BottomWall").transform.position = new Vector3(0, -viewHeight, 0);
    }
}