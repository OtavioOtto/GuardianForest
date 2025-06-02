using System.Collections;
using UnityEngine;

public class BridgeInteractionHandler : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject questionTxt;
    public GameObject crosshair;
    public GameObject stats;
    public GameObject player;
    [SerializeField] private GameObject animator;
    public GameObject buttons;
    [SerializeField] private Animator anim;
    [Header("Cam Move Values")]
    public Transform camDefaultValues;
    [SerializeField] private Transform camTarget;
    [SerializeField] private float transitionDuration = 1f;
    [Header("Verifiers")]
    [SerializeField] private bool playerInside;
    public bool isItTransitioning;

    private SecondPuzzleVerifier puzzle;

    public Coroutine cameraMovementCoroutine;

    private void Start()
    {
        isItTransitioning = false;
        puzzle = gameObject.GetComponent<SecondPuzzleVerifier>();
        puzzle.enabled = false;
    }
    private void Update()
    {
        InteractionHandler(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questionTxt.SetActive(true);
            playerInside = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questionTxt.SetActive(false);
            playerInside = false;
        }
    }

    void InteractionHandler()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !isItTransitioning)
        {
            if (questionTxt.activeSelf)
            {
                Camera.main.transform.parent = null;
                questionTxt.SetActive(false);
                crosshair.SetActive(false);
                stats.SetActive(false);
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<SpearShooter>().enabled = false;
                player.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
                anim.SetBool("andando", false);
                anim.SetBool("lancou", false);
                animator.GetComponent<CurupiraAnimations>().enabled = false;

                if (cameraMovementCoroutine != null)
                    StopCoroutine(cameraMovementCoroutine);
                isItTransitioning=true;
                cameraMovementCoroutine = StartCoroutine(MoveCamera(camTarget.position, camTarget.rotation, false));
                puzzle.enabled = true;
            }
            else
            {
                buttons.SetActive(false);
                Camera.main.transform.parent = player.transform;
                questionTxt.SetActive(true);
                crosshair.SetActive(true);
                stats.SetActive(true);

                if (cameraMovementCoroutine != null)
                    StopCoroutine(cameraMovementCoroutine);
                isItTransitioning = true;
                cameraMovementCoroutine = StartCoroutine(MoveCamera(camDefaultValues.position, camDefaultValues.rotation, true));
                
                
            }
        }
    }

    public IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation, bool isItReturning)
    {

        Transform cameraTransform = Camera.main.transform;
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            cameraTransform.position = Vector3.Lerp(startPos, targetPosition, t);
            cameraTransform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
            yield return null;
        }   

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;
        isItTransitioning = false;
        if(!isItReturning)
            buttons.SetActive(true);
        if (isItReturning)
        {
            
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<SpearShooter>().enabled = true;
            animator.GetComponent<CurupiraAnimations>().enabled = true;

            if (puzzle.puzzleDone && this.gameObject.GetComponent<SphereCollider>().enabled == true)
            {
                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                questionTxt.SetActive(false);
                playerInside = false;
            }
            puzzle.enabled = false;

        }

        
    }
}