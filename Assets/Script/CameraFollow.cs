using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -6f); 
    [SerializeField] private float smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(player.position + Vector3.up * 1f);
    }
}