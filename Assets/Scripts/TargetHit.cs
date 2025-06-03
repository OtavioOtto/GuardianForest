using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public bool hit;

    private TargetHit parent;
    public TargetsPuzzleHandler puzzle;
    private void Start()
    {
        hit = false;
        if(transform.parent.GetComponent<TargetHit>() != null)
            parent = transform.parent.GetComponent<TargetHit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(puzzle.activateTimer)
            if (other.CompareTag("Bullet")) 
            {
                if (parent != null)
                {
                    parent.hit = true;
                    hit = true;
                }
                else
                    hit = true;
            }
    }
}
