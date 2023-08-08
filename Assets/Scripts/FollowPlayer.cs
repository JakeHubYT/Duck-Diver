using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public Vector2 followOffset; // Offset on the X and Y axes
    public float followSpeed = 5f; // Speed at which the object follows the player

    void Update()
    {
        // Check if the player reference is set
        if (player == null)
        {
            Debug.LogWarning("Player reference is not set in the FollowPlayer script.");
            return;
        }

        // Calculate the target position with the offset on the X and Y axes
        Vector3 targetPosition = new Vector3(player.position.x + followOffset.x, player.position.y + followOffset.y, transform.position.z);

        // Move the object towards the target position smoothly using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
