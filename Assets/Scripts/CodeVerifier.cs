using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeVerifier : MonoBehaviour
{
    public Transform verifier;
    public Transform wrong;
    public Transform target;
    public GameObject cubeCheck;
    public GameObject sphereCheck;
    public GameObject cylinderCheck;
    public GameObject txtRestart;
    public float speed;

    void Update()
    {
        if (sphereCheck.activeSelf)
        {
            if (cylinderCheck.activeSelf)
            {
                if (cubeCheck.activeSelf)
                {
                    verifier.position = Vector3.MoveTowards(verifier.position, target.position, speed * Time.deltaTime);
                    txtRestart.SetActive(true);
                }
            }
        }
        if ((cylinderCheck.activeSelf && !sphereCheck.activeSelf) || (cubeCheck.activeSelf && !sphereCheck.activeSelf))
        {
            txtRestart.SetActive(true);
            wrong.position = Vector3.MoveTowards(wrong.position, target.position, speed * Time.deltaTime);
        }
        if (cubeCheck.activeSelf && !cylinderCheck.activeSelf) 
        {
            txtRestart.SetActive(true);
            wrong.position = Vector3.MoveTowards(wrong.position, target.position, speed * Time.deltaTime);
        }
    }
}
