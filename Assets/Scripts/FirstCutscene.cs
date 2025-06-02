using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class FirstCutscene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToScene());
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(2);
    }
    IEnumerator GoToScene() 
    {
        yield return new WaitForSeconds(80f);
        SceneManager.LoadScene(2);
    }
}
