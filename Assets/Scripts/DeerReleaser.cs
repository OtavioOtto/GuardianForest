using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeerReleaser : MonoBehaviour
{
    public GameObject cage;
    public Transform cageT;
    private Transform cageTransf;
    public GameObject deer;
    public GameObject secondPuzzleObstacle;
    public GameObject secondPuzzleEnemies;
    [SerializeField] private string eachHole;
    [Header("First Hole")]
    [SerializeField] private GameObject firstHoleLimits;
    [SerializeField] private GameObject firstStone;
    [SerializeField] private GameObject firstTrapDoor;
    [Header("Second Hole")]
    [SerializeField] private GameObject secondHoleLimits;
    [SerializeField] private GameObject secondStone;
    [SerializeField] private GameObject secondTrapDoor;
    [Header("Third Hole")]
    [SerializeField] private GameObject thirdHoleLimits;
    [SerializeField] private GameObject thirdStone;
    [SerializeField] private GameObject thirdTrapDoor;

    private void Start()
    {
        cageTransf = cageT;
        eachHole = gameObject.transform.name;
        firstTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            CodeVerifier();
        }
    }

    private void CodeVerifier() 
    {

        if (eachHole.Equals("Hole1"))
        {

            firstHoleLimits.SetActive(false);
            firstStone.GetComponent<Outline>().enabled = false;
            firstTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();

            secondHoleLimits.SetActive(true);
            secondStone.SetActive(true);
            secondTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();
        }
        else if (eachHole.Equals("Hole2"))
        {

            secondHoleLimits.SetActive(false);
            secondStone.GetComponent<Outline>().enabled = false;
            secondTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();

            thirdHoleLimits.SetActive(true);
            thirdStone.SetActive(true);
            thirdTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();
        }
        else
        {
            thirdHoleLimits.SetActive(false);
            thirdStone.GetComponent<Outline>().enabled = false;
            thirdTrapDoor.GetComponent<TrapdoorController>().ToggleTrapdoor();

            cageTransf.position = new Vector3(cageT.position.x, cageT.position.y + 0.8f, cageT.position.z);
            Destroy(cage);
            GameObject deerGO = Instantiate(deer, cageTransf.position, cageTransf.rotation);
            deerGO.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            Destroy(secondPuzzleObstacle);
            secondPuzzleEnemies.SetActive(true);
        }
        
    }
}
