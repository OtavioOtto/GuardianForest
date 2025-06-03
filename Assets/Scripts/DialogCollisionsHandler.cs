using UnityEngine;

public class DialogCollisionsHandler : MonoBehaviour
{
    public bool playerInside;
    public bool firsTime;

    private void Start()
    {
        playerInside = false;
        firsTime = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
