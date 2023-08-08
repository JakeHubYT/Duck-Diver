using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool isFacingRight = true;
    public bool isWalking = false;
    public bool isMovingRight = true;
    public float walkDuration = 2f;
    public float idleDuration = 1f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    public Animator anim;

    void Start()
    {
        StartCoroutine(RandomWalk());
    }

    IEnumerator RandomWalk()
    {
        while (true)
        {
            // Wait for the idle duration.
            yield return new WaitForSeconds(idleDuration);

            // Randomly choose whether to walk left or right.
            isMovingRight = Random.Range(0, 2) == 1;

            // Set the walking state to true.
            isWalking = true;

            // Wait for the walk duration.
            yield return new WaitForSeconds(walkDuration);

            // Set the walking state to false.
            isWalking = false;

            // Wait for the idle duration again before the next walk.
            yield return new WaitForSeconds(idleDuration);
        }
    }

    void Update()
    {
        if (isWalking)
        {
            // Move the AI left or right based on the isMovingRight flag.
            float horizontalInput = isMovingRight ? 1f : -1f;
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Rotate the AI slowly to give the appearance of turning.
            float targetRotation = isMovingRight ? 180f : 0f;
            float step = rotationSpeed * Time.deltaTime;
            float newRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, step);
            transform.eulerAngles = new Vector3(0f, newRotation, 0f);
        }

        anim.SetBool("isWalking", isWalking);
    }
}
