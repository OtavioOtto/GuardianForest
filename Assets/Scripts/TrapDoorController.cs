using UnityEngine;

public class TrapdoorController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float openAngle = 90f; 
    public float rotationSpeed = 90f;

    [Header("State")]
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private float rotationProgress = 0f;

    void Start()
    { 
        closedRotation = transform.rotation;

        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {

        if (isOpen)
        {
            rotationProgress = Mathf.Min(rotationProgress + Time.deltaTime * rotationSpeed / openAngle, 1f);
        }
        else
        {
            rotationProgress = Mathf.Max(rotationProgress - Time.deltaTime * rotationSpeed / openAngle, 0f);
        }

        transform.rotation = Quaternion.Slerp(closedRotation, openRotation, rotationProgress);
    }

    // Public method to toggle the trapdoor
    public void ToggleTrapdoor()
    {
        isOpen = !isOpen;
    }
}