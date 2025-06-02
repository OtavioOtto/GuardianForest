using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalCutscene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToScene());
    }

    IEnumerator GoToScene()
    {
        yield return new WaitForSeconds(22f);
        SceneManager.LoadScene(0);
    }
}
