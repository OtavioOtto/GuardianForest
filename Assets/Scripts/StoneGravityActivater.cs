using UnityEngine;
using System.Collections;

public class StoneGravityActivater : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject stoneOne;
    [SerializeField] private GameObject stoneTwo;
    [SerializeField] private GameObject stoneThree;
    [Header("Rigidbodys")]
    [SerializeField] private Rigidbody stoneOneRB;
    [SerializeField] private Rigidbody stoneTwoRB;
    [SerializeField] private Rigidbody stoneThreeRB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            StartCoroutine(DisableConstraints(other));
        }
       
    }

   
    IEnumerator DisableConstraints(Collider other)
    {
        yield return new WaitForSeconds(.1f);
        if(other.gameObject == stoneOne)
            stoneOneRB.constraints = RigidbodyConstraints.None;
        if (other.gameObject == stoneTwo)
            stoneTwoRB.constraints = RigidbodyConstraints.None;
        if (other.gameObject == stoneThree)
            stoneThreeRB.constraints = RigidbodyConstraints.None;
    }

}
