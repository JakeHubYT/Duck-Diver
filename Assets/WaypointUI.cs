using UnityEngine;

public class WaypointUI : MonoBehaviour
{
    public Transform targetWaypoint; // The waypoint's Transform in world space

    void Update()
    {
        if (targetWaypoint == null)
        {
            // Hide the UI element if the waypoint is not set
            gameObject.SetActive(false);
            return;
        }

        // Calculate the direction from the UI element to the waypoint on the x and y axes
        Vector3 directionToWaypoint = targetWaypoint.position - transform.position;
        directionToWaypoint.y = 0f; // Ignore the vertical component (y-axis)

        if (directionToWaypoint != Vector3.zero)
        {
            // Calculate the rotation angle between the UI element's forward (z-axis) and the direction of the waypoint (x and y axes only)
            float rotationAngle = Mathf.Atan2(directionToWaypoint.x, directionToWaypoint.y) * Mathf.Rad2Deg;

            // Apply the rotation to the UI element's z-axis only
            transform.eulerAngles = new Vector3(0f, 0f, -rotationAngle);
        }
    }
}
