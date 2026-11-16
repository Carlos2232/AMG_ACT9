using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings (Left, Middle, Right)")]
    [SerializeField] private float[] laneXPositions = new float[] { -2.5f, 0f, 2.5f };
    private int currentLaneIndex = 1;
    [SerializeField] private float laneChangeSpeed = 20f;
    private float targetX;

    [Header("Lane Switch Bounce / Juice")]
    [SerializeField] private float bounceHeight = 0.3f;
    private float bounceTimer = 0f;
    private bool isBouncing = false;

    [Header("Subway Surfers 3D Jump Settings")]
    [SerializeField] private float jumpHeight = 2.5f;     // Vertical height of the jump
    [SerializeField] private float jumpDuration = 0.5f;
    private float jumpTimer = 0f;
    private bool isJumping = false;
    private float jumpStartY = -3.5f;

    [Header("Hit Detection Settings")]
    [SerializeField] private float hitYThreshold = 0.6f;
    private float baseY = -3.5f;

    private void Start()
    {
        targetX = laneXPositions[currentLaneIndex];
        transform.position = new Vector3(targetX, baseY, 0f);
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentLaneIndex > 0)
        {
            currentLaneIndex--;
            targetX = laneXPositions[currentLaneIndex];
            TriggerLaneBounce();
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentLaneIndex < laneXPositions.Length - 1)
        {
            currentLaneIndex++;
            targetX = laneXPositions[currentLaneIndex];
            TriggerLaneBounce();
        }

        // Jump Input
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
            jumpStartY = transform.position.y;
        }
    }

    private void TriggerLaneBounce()
    {
        isBouncing = true;
        bounceTimer = 0f;
    }

    private void HandleMovement()
    {
        float currentX = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        float currentY = baseY;

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float progress = jumpTimer / jumpDuration;

            if (progress < 1f)
            {
                // Parabolic Arc Jump
                currentY = jumpStartY + (4f * jumpHeight * progress * (1f - progress));

                // 3D Jump Juice: Scale up slightly as if leaping closer toward the camera screen
                float jumpScale = Mathf.Lerp(1f, 1.35f, Mathf.Sin(progress * Mathf.PI));
                transform.localScale = Vector3.one * jumpScale;
            }
            else
            {
                isJumping = false;
                currentY = baseY;
                transform.localScale = Vector3.one;
            }
        }
        else if (isBouncing)
        {
            bounceTimer += Time.deltaTime * 18f;
            float bounceOffset = Mathf.Sin(bounceTimer) * bounceHeight;
            currentY = baseY + bounceOffset;

            float tiltAngle = Mathf.Sin(bounceTimer) * 10f;
            transform.rotation = Quaternion.Euler(0f, 0f, -tiltAngle * (targetX > transform.position.x ? 1f : -1f));

            if (bounceTimer >= Mathf.PI)
            {
                isBouncing = false;
                transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        transform.position = new Vector3(currentX, currentY, 0f);
    }

    public bool CheckHit(float obstacleY, int obstacleLane)
    {
        bool isAtPlayerY = Mathf.Abs(obstacleY - baseY) <= hitYThreshold;
        bool isInSameLane = (obstacleLane == currentLaneIndex);

        if (isAtPlayerY && isInSameLane)
        {
            // If you are high enough in the air during your jump arc, you successfully clear the wall!
            if (transform.position.y > (baseY + 1.0f))
            {
                return false; // Cleared! No damage.
            }
            return true; // Crash! Hit the wall (-5 HP)
        }
        return false;
    }
}