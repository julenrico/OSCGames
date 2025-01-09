using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")] public Transform player;
    public Transform opponent;

    [Header("Position Settings")] public Vector3 offset = new Vector3(0, 5, -8);
    public float followSpeed = 5f;

    [Header("Zoom Settings")] public float minDistance = 5f;
    public float maxDistance = 15f;
    public float minFOV = 60f;
    public float maxFOV = 90f;
    public float zoomSpeed = 5f;

    private Camera cam;

    void Start()
    {
        // Get the Camera component
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null || opponent == null) return;

        // Calculate midpoint and distance
        Vector3 midpoint = (player.position + opponent.position) / 2f;
        float distance = Vector3.Distance(player.position, opponent.position);

        // Adjust camera position
        Vector3 targetPosition = midpoint + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Adjust FOV based on distance
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, (distance - minDistance) / (maxDistance - minDistance));
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);

        // Ensure the camera is always looking at the midpoint
        transform.LookAt(midpoint);
    }
}