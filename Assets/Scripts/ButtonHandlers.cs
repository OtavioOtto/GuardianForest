using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlers : MonoBehaviour
{
    public GameObject triangle;
    public GameObject square;
    public GameObject circle;
    public Button triangleButton;
    public Button squareButton;
    public Button circleButton;
    public GameObject buttons;

    public void Triangle() 
    {
        triangle.SetActive(true);
        triangleButton.interactable = false;
    }
    public void Square() 
    {
        square.SetActive(true);
        squareButton.interactable = false;
    }
    public void Circle() 
    {
        circle.SetActive(true);
        circleButton.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && buttons.activeSelf) 
        {
            triangleButton.interactable = true;
            squareButton.interactable = true;
            circleButton.interactable = true;
            square.SetActive(false);
            triangle.SetActive(false);
            circle.SetActive(false);
            buttons.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

    }
}
