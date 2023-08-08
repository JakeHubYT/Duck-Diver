using UnityEngine;

public class PointAtWaypoint : MonoBehaviour
{

    public Transform player; // Assign the player's GameObject Transform here.
    public Transform target; // Assign the quest target's GameObject Transform here.

    void Update()
    {
        // Check if both player and target are assigned.
        if (player == null || target == null)
            return;

        // Calculate the direction vector from the player to the target.
        Vector3 direction = (target.position - player.position).normalized;

        // Calculate the rotation to point the arrow in the direction of the target.
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Apply the rotation to the arrow pointer.
        transform.rotation = targetRotation;
    }

}
