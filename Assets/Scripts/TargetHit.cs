using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public bool hit;
    private void Start()
    {
        hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) 
        {
            hit = true;
        }
    }
}
