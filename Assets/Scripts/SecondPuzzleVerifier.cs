using UnityEngine;

public class SecondPuzzleVerifier : MonoBehaviour
{
    [Header("Log1")]
    [SerializeField] private GameObject logOneOptionOne;
    [SerializeField] private GameObject logOneOptionTwo;
    [SerializeField] private GameObject logOneOptionThree;

    [Header("Log2")]
    [SerializeField] private GameObject logTwoOptionOne;
    [SerializeField] private GameObject logTwoOptionTwo;
    [SerializeField] private GameObject logTwoOptionThree;

    [Header("Log3")]
    [SerializeField] private GameObject logThreeOptionOne;
    [SerializeField] private GameObject logThreeOptionTwo;
    [SerializeField] private GameObject logThreeOptionThree;

    [Header("Puzzle")]
    [SerializeField] private GameObject stumpOne;
    [SerializeField] private GameObject stumpTwo;
    [SerializeField] private GameObject stumpThree;
    [SerializeField] private GameObject hints;
    [SerializeField] private GameObject obstacle;

    [Header("Bridge")]
    [SerializeField] private GameObject bridge;

    [Header("Particles")]
    [SerializeField] private GameObject splash;
    [SerializeField] private Transform splashPos;
    [SerializeField] private GameObject waterWave;
    [SerializeField] private Transform waterWavePos;

    [Header("Verifiers")]
    public int attempts;
    public bool puzzleDone;

    private BridgeInteractionHandler interaction;

    private void Start()
    {
        attempts = 0;
        puzzleDone = false;
        interaction = gameObject.GetComponent<BridgeInteractionHandler>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (logOneOptionTwo.activeSelf && logTwoOptionOne.activeSelf && logThreeOptionThree.activeSelf)
            {
                logOneOptionTwo.SetActive(false);
                logTwoOptionOne.SetActive(false);
                logThreeOptionThree.SetActive(false);

                stumpOne.SetActive(false);
                stumpTwo.SetActive(false);
                stumpThree.SetActive(false);

                hints.SetActive(false);

                obstacle.SetActive(false);

                GameObject splashVFX = Instantiate(splash, splashPos);
                splashVFX.transform.localScale = new Vector3(10,10,10);

                GameObject waterWaveVFX = Instantiate (waterWave, waterWavePos);
                waterWaveVFX.transform.localScale = new Vector3 (10,10,10);

                bridge.SetActive(true);
                puzzleDone = true;

                interaction.buttons.SetActive(false);
                Camera.main.transform.parent = interaction.player.transform;
                interaction.crosshair.SetActive(true);
                interaction.stats.SetActive(true);

                if (interaction.cameraMovementCoroutine != null)
                    StopCoroutine(interaction.cameraMovementCoroutine);
                interaction.isItTransitioning = true;
                StartCoroutine(interaction.MoveCamera(interaction.camDefaultValues.position, interaction.camDefaultValues.rotation, true));
            }

            else
                PuzzleReset();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            LogsHandler(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            LogsHandler(2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            LogsHandler(3);
    }

    void PuzzleReset() 
    {
        logOneOptionOne.SetActive(false);
        logOneOptionTwo.SetActive(false);
        logOneOptionThree.SetActive(false);

        logTwoOptionOne.SetActive(false);
        logTwoOptionTwo.SetActive(false);
        logTwoOptionThree.SetActive(false);

        logThreeOptionOne.SetActive(false);
        logThreeOptionTwo.SetActive(false);
        logThreeOptionThree.SetActive(false);
        attempts++;
    }

    void LogsHandler(int log) 
    {
        if (log == 1)
        {
            if (!(logThreeOptionOne.activeSelf || logTwoOptionOne.activeSelf))
                logOneOptionOne.SetActive(true);

            else if ((logThreeOptionOne.activeSelf && !logTwoOptionTwo.activeSelf) || (logTwoOptionOne.activeSelf && !logThreeOptionTwo.activeSelf))
                logOneOptionTwo.SetActive(true);

            else if ((logTwoOptionOne.activeSelf && logThreeOptionTwo.activeSelf) || (logThreeOptionOne.activeSelf && logTwoOptionTwo.activeSelf))
                logOneOptionThree.SetActive(true);
        }

        else if (log == 2)
        {
            if (!(logThreeOptionOne.activeSelf || logOneOptionOne.activeSelf))
                logTwoOptionOne.SetActive(true);

            else if ((logThreeOptionOne.activeSelf && !logOneOptionTwo.activeSelf) || (logOneOptionOne.activeSelf && !logThreeOptionTwo.activeSelf))
                logTwoOptionTwo.SetActive(true);

            else if ((logOneOptionOne.activeSelf && logThreeOptionTwo.activeSelf) || (logThreeOptionOne.activeSelf && logOneOptionTwo.activeSelf))
                logTwoOptionThree.SetActive(true);
        }

        else if (log == 3) 
        {
            if (!(logOneOptionOne.activeSelf || logTwoOptionOne.activeSelf))
                logThreeOptionOne.SetActive(true);

            else if ((logOneOptionOne.activeSelf && !logTwoOptionTwo.activeSelf) || (logTwoOptionOne.activeSelf && !logOneOptionTwo.activeSelf))
                logThreeOptionTwo.SetActive(true);

            else if ((logTwoOptionOne.activeSelf && logOneOptionTwo.activeSelf) || (logOneOptionOne.activeSelf && logTwoOptionTwo.activeSelf))
                logThreeOptionThree.SetActive(true);
        }
    }
}
