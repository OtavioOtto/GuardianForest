using UnityEngine;

public class SpearShooter : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform curvePoint;
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private Rigidbody spearRB;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Animator anim;

    [Header("Values")]
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;
    [SerializeField] private Vector3 old_pos;

    [Header("Settings")]
    public bool isReturning;
    public bool playerHasSpear;
    [SerializeField] private float time;

    private GameObject spearInstance;

    private SpearAddOns addOns;

    private void Start()
    {
        playerHasSpear = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !playerHasSpear && Time.timeScale != 0 && !isReturning)
            ReturningMethod();
        if (isReturning)
        {
            if (time < 1.0f)
            {
                spearInstance.transform.position = ReturnCalculus(time, old_pos, curvePoint.position, rightHand.position);
                time += Time.deltaTime;
            }
            else
                ResetSpear();
        }

        if(addOns != null)
            if (!playerHasSpear && !addOns.isHit && !isReturning && !anim.GetBool("atacou") && !spear.activeSelf)
                spearInstance.transform.forward = Vector3.Slerp(transform.forward, spearInstance.GetComponent<Rigidbody>().linearVelocity.normalized, Time.deltaTime);

    }

    public void ShootingMethod()
    {
        spear.SetActive(false);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction = ray.direction.normalized;

        Vector3 spawnPos = (cam.position + rightHand.position) * 0.5f;

        spearInstance = Instantiate(spearPrefab, spawnPos, Quaternion.LookRotation(direction));
        spearRB = spearInstance.GetComponent<Rigidbody>();
        spearRB.AddForce(spearInstance.transform.forward * throwForce, ForceMode.Impulse);

        playerHasSpear = false;
        isReturning = false;

        addOns = spearInstance.GetComponent<SpearAddOns>();
    }

    void ReturningMethod() 
    {
        spearRB = spearInstance.GetComponent<Rigidbody>();
        spearRB.isKinematic = false;
        spearInstance.GetComponent<CapsuleCollider>().enabled = false;
        spearInstance.GetComponentInChildren<TrailRenderer>().enabled = false;
        time = 0.0f;
        old_pos = spearInstance.transform.position;
        isReturning = true;
        spearRB.linearVelocity = Vector3.zero;
        
    }
    void ResetSpear()
    {
        Destroy(spearInstance);
        spear.SetActive(true);
        isReturning = false;
        playerHasSpear = true;
        addOns.isHit = false;
        anim.SetBool("lancou", false);
    }
    Vector3 ReturnCalculus(float t, Vector3 p0, Vector3 p1, Vector3 p2) {

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + tt * p2;
        return p;


    }
}
