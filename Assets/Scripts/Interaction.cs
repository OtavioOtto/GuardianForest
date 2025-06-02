using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    [SerializeField] private bool playerInside = false;
    [SerializeField] private GameObject child;
    [SerializeField] private TMP_Text aviso;
    private void Update()
    {
        if(playerInside && Input.GetKeyDown(KeyCode.E))
        {
            child.SetActive(true);
            aviso.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !child.activeSelf)
        {
            aviso.gameObject.SetActive(true);
            playerInside = true;
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aviso.gameObject.SetActive(false);
            playerInside = false;
        }
    }
}
