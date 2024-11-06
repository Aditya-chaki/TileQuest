using UnityEngine;

public class MapCameraControllerMobile : MonoBehaviour
{
    // Movement settings
    public float panSpeed = 0.5f;
    public Vector2 panLimit = new Vector2(50, 50); // Limits on the map movement

    // Zoom settings
    public float zoomSpeed = 0.1f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    // Smooth movement (optional)
    private Vector3 targetPosition;

    void Start()
    {
        // Initialize the target position to the camera's starting position
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleTouchMovement();
        HandlePinchZoom();

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panSpeed);
    }

    void HandleTouchMovement()
    {
        // Check if the user is touching with a single finger to pan
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get the touch delta position
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            
            // Update target position based on touch movement
            targetPosition -= new Vector3(touchDeltaPosition.x * panSpeed * Time.deltaTime, 
                                          touchDeltaPosition.y * panSpeed * Time.deltaTime, 0);

            // Clamping position within the specified pan limits
            targetPosition.x = Mathf.Clamp(targetPosition.x, -panLimit.x, panLimit.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -panLimit.y, panLimit.y);
        }
    }

    void HandlePinchZoom()
    {
        // Check if the user is pinching with two fingers to zoom
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Find the position in the previous frame of each touch
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // Calculate the magnitude of the vector (distance) between the touches in each frame
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            // Calculate the difference in distance between frames
            float difference = currentMagnitude - prevMagnitude;

            // Adjust orthographic size for 2D or field of view for 3D
            Camera.main.orthographicSize -= difference * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            
            // For 3D camera, use field of view:
            // Camera.main.fieldOfView -= difference * zoomSpeed;
            // Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
        }
    }
}
