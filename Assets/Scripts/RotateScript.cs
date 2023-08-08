using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis axisToRotate = RotationAxis.Y;
    public float rotationSpeed = 60f;

    void Update()
    {
        // Get the rotation axis based on the user's selection
        Vector3 axis;
        switch (axisToRotate)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;
            case RotationAxis.Y:
                axis = Vector3.up;
                break;
            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
            default:
                axis = Vector3.up;
                break;
        }

        // Rotate the object around the selected axis
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
