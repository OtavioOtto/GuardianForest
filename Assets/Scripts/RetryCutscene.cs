using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RetryCutscene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToScene());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator GoToScene()
    {
        yield return new WaitForSeconds(19f);
        SceneManager.LoadScene(2);
    }
}
