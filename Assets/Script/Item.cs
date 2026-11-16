using UnityEngine;

public class Item : MonoBehaviour
{
    public int assignedLane;
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

        if (player != null)
        {
            transform.SetSiblingIndex(player.transform.GetSiblingIndex());
        }
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        float progress = Mathf.InverseLerp(spawnY, playerY, transform.position.y);
        float laneMultiplier = assignedLane - 1;
        float currentSpread = Mathf.Lerp(0.2f, trackWidthAtBottom, progress);
        float calculatedX = laneMultiplier * currentSpread;

        transform.position = new Vector3(calculatedX, transform.position.y, 0f);

        float perspectiveScale = Mathf.Lerp(0.2f, 1.2f, progress);
        transform.localScale = Vector3.one * perspectiveScale;

        if (!hasPassedPlayer && player != null)
        {
            if (transform.position.y <= player.transform.position.y)
            {
                hasPassedPlayer = true;
                transform.SetAsLastSibling();
            }
        }

        if (!hasHitPlayer && GameManager.Instance != null && player != null)
        {
            if (player.CheckHit(transform.position.y, assignedLane))
            {
                hasHitPlayer = true;
                GameManager.Instance.TakeDamage(5); 
            }
        }

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}