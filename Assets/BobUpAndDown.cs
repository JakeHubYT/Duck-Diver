using UnityEngine;

public class BobUpAndDown : MonoBehaviour
{
    public float bobbingSpeed = 1.0f;    // Speed of the bobbing motion.
    public float bobbingHeight = 0.5f;   // Height of the bobbing motion.
    public float bobbingOffset = 0.0f;   // Offset to control starting position.

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Calculate the new Y position using a sine wave.
        float newY = startPos.y + Mathf.Sin((Time.time + bobbingOffset) * bobbingSpeed) * bobbingHeight;

        // Update the object's position with the new Y value.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
