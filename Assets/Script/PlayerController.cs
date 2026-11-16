using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float[] laneXPositions = new float[] { -3.5f, 0f, 3.5f };
    private int currentLaneIndex = 1;
    [SerializeField] private float laneChangeSpeed = 20f;
    private float targetX;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float jumpDuration = 0.5f;
    private float jumpTimer = 0f;
    private bool isJumping = false;
    private float jumpStartY = -3.5f;
    [SerializeField] private float hitYThreshold = 0.6f;
    private float baseY = -3.5f;

    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.25f;
    private SpriteRenderer spriteRenderer;
    private Image uiImage;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        targetX = laneXPositions[currentLaneIndex];
        transform.position = new Vector3(targetX, baseY, 0f);

        spriteRenderer = GetComponent<SpriteRenderer>();
        uiImage = GetComponent<Image>();

        if (spriteRenderer != null) originalColor = spriteRenderer.color;
        else if (uiImage != null) originalColor = uiImage.color;
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
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentLaneIndex < laneXPositions.Length - 1)
        {
            currentLaneIndex++;
            targetX = laneXPositions[currentLaneIndex];
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
            jumpStartY = transform.position.y;
        }
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
                currentY = jumpStartY + (4f * jumpHeight * progress * (1f - progress));
            }
            else
            {
                isJumping = false;
                currentY = baseY;
            }
        }

        transform.position = new Vector3(currentX, currentY, 0f);
    }

    public bool CheckHit(float obstacleY, int obstacleLane)
    {
        bool isAtPlayerY = Mathf.Abs(obstacleY - baseY) <= hitYThreshold;
        bool isInSameLane = (obstacleLane == currentLaneIndex);

        if (isAtPlayerY && isInSameLane)
        {
            if (transform.position.y > (baseY + 1.0f))
            {
                return false;
            }

            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FlashRed());
            return true;
        }
        return false;
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null) spriteRenderer.color = hitColor;
        if (uiImage != null) uiImage.color = hitColor;

        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null) spriteRenderer.color = originalColor;
        if (uiImage != null) uiImage.color = originalColor;
    }
}