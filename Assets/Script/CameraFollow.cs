using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -6f); // Behind and above the player
    [SerializeField] private float smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null) return;

        // Desired position behind the player
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Look slightly ahead of the player down the track
        transform.LookAt(player.position + Vector3.up * 1f);
    }
}