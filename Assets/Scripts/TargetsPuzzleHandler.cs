using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetsPuzzleHandler : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject avisoTxt;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossObstacle;
    [SerializeField] private GameObject thirdEnemyGroup;
    [SerializeField] private Transform playerPos;

    [Header("Timer Properties")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private TMP_Text countTxt;
    [SerializeField] private float gameTime;
    [SerializeField] private bool stopTimer;

    public bool puzzleComplete;
    public bool activateTimer;
    private bool playerInside;
    private float currentTime;
    private bool puzzleStart;

    void Start()
    {
        playerInside = false;
        stopTimer = false;
        slider.maxValue = gameTime;
        slider.value = gameTime;
        activateTimer = false;
        currentTime = gameTime;
        puzzleStart = false;
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !puzzleComplete && !puzzleStart)
        {
            avisoTxt.SetActive(false);
            obstacle.SetActive(true);
            countTxt.gameObject.SetActive(true);
            activateTimer = true;
            player.transform.position = playerPos.position;
            currentTime = gameTime;
            puzzleStart = true;
        }

        if(puzzleStart && !stopTimer)
            PuzzleStart();

        if (activateTimer && !stopTimer)
            TimeHandler();

        if (stopTimer)
        {
            if (!puzzleComplete)
            {
                avisoTxt.SetActive(true);

                int count = gameObject.transform.childCount;

                for (int i = 0; i < count; i++)
                {
                    gameObject.transform.GetChild(i).GetComponentInChildren<TargetHit>().hit = false;
                }
            }
            else
            {
                bossObstacle.SetActive(false);
                thirdEnemyGroup.SetActive(true);
            }

            obstacle.SetActive(false);
            countTxt.gameObject.SetActive(false);
            activateTimer = false;
            currentTime = gameTime;
            puzzleStart = false;
            slider.gameObject.SetActive(false);
            stopTimer = false;

            
        }
    }

    void TimeHandler() 
    {
        slider.gameObject.SetActive(true);
        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (currentTime <= 0)
        {
            currentTime = 0;
            avisoTxt.SetActive(true);
            stopTimer = true;
            obstacle.SetActive(false);
            activateTimer = false;
        }

        timerTxt.text = textTime;
        slider.value = currentTime;
    }

    void PuzzleStart()
    {
        int count = gameObject.transform.childCount;
        countTxt.text = "Alvos Restantes: " + count;

        float remainingTargets = 0;

        for (int i = 0; i < count; i++)
        {
            if (!gameObject.transform.GetChild(i).GetComponentInChildren<TargetHit>().hit)
            {
                remainingTargets++;
            }

        }

        countTxt.text = "Alvos Restantes: " + remainingTargets;

        if (remainingTargets == 0)
        {
            puzzleComplete = true;
            stopTimer = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            activateTimer = false;
            slider.gameObject.SetActive(false);
        }
    }
}