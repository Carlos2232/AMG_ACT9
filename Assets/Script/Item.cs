using UnityEngine;

public class Item : MonoBehaviour
{
    public int assignedLane; // 0 = Left, 1 = Middle, 2 = Right
    public float speed = 5f;

    [Header("Subway Surfers Cone Settings")]
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float playerY = -3.5f;
    [SerializeField] private float trackWidthAtBottom = 4f;

    private bool hasHitPlayer = false;
    private bool hasPassedPlayer = false;
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // Ensure obstacles start behind the player when spawned
        if (player != null)
        {
            // Place it right before the player in the hierarchy so it renders behind
            transform.SetSiblingIndex(player.transform.GetSiblingIndex());
        }
    }

    private void Update()
    {
        // 1. Move straight down toward the player (decreasing Y)
        transform.position += Vector3.down * speed * Time.deltaTime;

        // 2. Subway Surfers Perspective Cone Math
        float progress = Mathf.InverseLerp(spawnY, playerY, transform.position.y);
        float laneMultiplier = assignedLane - 1;
        float currentSpread = Mathf.Lerp(0.2f, trackWidthAtBottom, progress);
        float calculatedX = laneMultiplier * currentSpread;

        transform.position = new Vector3(calculatedX, transform.position.y, 0f);

        // 3. Perspective Scaling (Grows larger as it approaches)
        float perspectiveScale = Mathf.Lerp(0.2f, 1.2f, progress);
        transform.localScale = Vector3.one * perspectiveScale;

        // 4. Dynamic Layering: Once it crosses past the player's Y position, jump to the front!
        if (!hasPassedPlayer && player != null)
        {
            if (transform.position.y <= player.transform.position.y)
            {
                hasPassedPlayer = true;
                // Move to the very end of the hierarchy list so it renders ON TOP of the player
                transform.SetAsLastSibling();
            }
        }

        // 5. Check collision against player at the bottom judgment line
        if (!hasHitPlayer && GameManager.Instance != null && player != null)
        {
            if (player.CheckHit(transform.position.y, assignedLane))
            {
                hasHitPlayer = true;
                GameManager.Instance.TakeDamage(5); // Removes 5 HP
            }
        }

        // 6. Clean up when it goes off screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}