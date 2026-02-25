using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -10);

    [Header("Clamp X")]
    [SerializeField] private bool clampX = true;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 100f;

    [Header("Clamp Y")]
    [SerializeField] private bool clampY = true;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        float clampedX = clampX ? Mathf.Clamp(desiredPosition.x, minX, maxX) : desiredPosition.x;
        float clampedY = clampY ? Mathf.Clamp(desiredPosition.y, minY, maxY) : desiredPosition.y;

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
    }
}