using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;       // Speed for arrow key movement
    public float zoomSpeed = 5f;       // Speed for zooming
    public float minZoom = 2f;         // Minimum zoom
    public float maxZoom = 20f;        // Maximum zoom

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Movement using Arrow Keys or WASD
        float moveX = Input.GetAxis("Horizontal"); // Left/Right arrow or A/D
        float moveY = Input.GetAxis("Vertical");   // Up/Down arrow or W/S

        Vector3 move = new Vector3(moveX, moveY, 0f);
        transform.position += move * moveSpeed * Time.deltaTime;

        // Zoom using mouse scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            cam.transform.position += cam.transform.forward * scroll * zoomSpeed;
            float distance = Vector3.Distance(cam.transform.position, Vector3.zero);
            if (distance < minZoom)
                cam.transform.position = Vector3.zero + cam.transform.forward * minZoom;
            if (distance > maxZoom)
                cam.transform.position = Vector3.zero + cam.transform.forward * maxZoom;
        }
    }
}
