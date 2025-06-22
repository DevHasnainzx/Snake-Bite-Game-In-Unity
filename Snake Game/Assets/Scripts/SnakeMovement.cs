using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveInterval = 0.2f;
    public float segmentDistance = 0.5f;
    public int initialSize = 4;

    [Header("References")]
    public Transform segmentPrefab;
    public Food foodScript;
    public Text scoreText;

    [Header("Sound Effects")]
    public AudioClip eatSound;
    public float eatSoundVolume = 0.7f;

    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection;
    private List<Transform> segments = new List<Transform>();
    private float nextMoveTime;
    private int score;
    private bool isAlive = true;
    private AudioSource audioSource;

    // Public properties for mobile controls
    public Vector2 CurrentDirection => direction;
    public bool IsAlive => isAlive;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        ResetState();
    }

    void Update()
    {
        if (!isAlive) return;

        // Keyboard input (for testing)
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W)) nextDirection = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.S)) nextDirection = Vector2.down;
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D)) nextDirection = Vector2.right;
            else if (Input.GetKeyDown(KeyCode.A)) nextDirection = Vector2.left;
        }
    }

    void FixedUpdate()
    {
        if (!isAlive || Time.time < nextMoveTime) return;

        nextMoveTime = Time.time + moveInterval;
        Move();
    }

    public void ChangeDirection(Vector2 newDirection)
    {
        // Prevent 180-degree turns
        if ((direction.x != 0 && newDirection.x != 0) ||
            (direction.y != 0 && newDirection.y != 0))
        {
            return;
        }
        nextDirection = newDirection;
    }

    void Move()
    {
        direction = nextDirection;

        // Move body segments
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Move head with grid snapping
        transform.position = new Vector2(
            Mathf.Round((transform.position.x + direction.x * segmentDistance) * 2) / 2,
            Mathf.Round((transform.position.y + direction.y * segmentDistance) * 2) / 2
        );

        CheckCollisions();
    }

    void Grow()
    {
        Transform newSegment = Instantiate(segmentPrefab);
        newSegment.position = segments[segments.Count - 1].position;
        segments.Add(newSegment);

        score++;
        scoreText.text = "Score: " + score;

        if (eatSound != null)
        {
            audioSource.PlayOneShot(eatSound, eatSoundVolume);
        }

        RespawnFood();
    }

    void RespawnFood()
    {
        bool validPosition;
        int attempts = 0;

        do
        {
            validPosition = true;
            foodScript.transform.position = new Vector2(
                Mathf.Round(Random.Range(-8, 8) * segmentDistance),
                Mathf.Round(Random.Range(-4, 4) * segmentDistance)
            );

            foreach (var segment in segments)
            {
                if (Vector2.Distance(foodScript.transform.position, segment.position) < segmentDistance)
                {
                    validPosition = false;
                    break;
                }
            }

            attempts++;
        } while (!validPosition && attempts < 100);
    }

    void CheckCollisions()
    {
        // Food collision
        if (Vector2.Distance(transform.position, foodScript.transform.position) < segmentDistance / 2)
        {
            Grow();
        }

        // Self collision
        for (int i = 1; i < segments.Count; i++)
        {
            if (Vector2.Distance(transform.position, segments[i].position) < segmentDistance / 2)
            {
                GameOver();
                return;
            }
        }
      
    }
    // Add this new method for wall collisions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            GameOver();
        }
    }
    void GameOver()
    {
        isAlive = false;
        enabled = false;
        FindObjectOfType<GameOverManager>().TriggerGameOver();
    }

    void ResetState()
    {
        direction = Vector2.right;
        nextDirection = Vector2.right;
        transform.position = Vector3.zero;
        score = 0;
        scoreText.text = "Score: 0";
        isAlive = true;
        enabled = true;

        // Clear old segments
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        // Create initial body
        for (int i = 1; i < initialSize; i++)
        {
            Grow();
        }

        RespawnFood();
    }
}