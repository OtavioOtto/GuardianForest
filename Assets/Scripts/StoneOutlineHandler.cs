using UnityEngine;

public class StoneOutlineHandler : MonoBehaviour
{
    private Outline outline;
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            outline.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            outline.enabled = false;
    }
}
