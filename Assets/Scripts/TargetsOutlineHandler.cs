using UnityEngine;

public class TargetsOutlineHandler : MonoBehaviour
{
    [SerializeField] private GameObject avisoTxt;
    private GameObject[] targets;
    private Outline outline;

    private TargetHit hit;
    private TargetsPuzzleHandler handler;
    public bool playerInside;
    void Start()
    {
        playerInside = false;
        handler = gameObject.GetComponent<TargetsPuzzleHandler>();

        int children = transform.childCount;
        targets = new GameObject[children];
        for (int i = 0; i < targets.Length; i++) 
        {
            targets[i] = transform.GetChild(i).gameObject;
            if (targets[i] != null)
            {
                outline = targets[i].GetComponent<Outline>();
                outline.enabled = false;
            }
        }
        
    }

    private void Update()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            outline = targets[i].GetComponent<Outline>();
            TargetHit hitVerifier = targets[i].GetComponentInChildren<TargetHit>();
            if (outline.enabled && hitVerifier.hit)
                outline.enabled = false;

            if (!outline.enabled && !hitVerifier.hit && playerInside)
                outline.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInside = true;
        if (other.CompareTag("Player") && !handler.puzzleComplete) 
        {
            for (int i = 0; i < targets.Length; i++)
            {
                outline = targets[i].GetComponent<Outline>();
                outline.enabled = true;
            }
            avisoTxt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                outline = targets[i].GetComponent<Outline>();
                outline.enabled = false;
            }
            avisoTxt.SetActive(false);
        }
    }
}
