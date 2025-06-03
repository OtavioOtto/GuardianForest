using TMPro;
using UnityEngine;
using System.Collections;

public class DialogsHandler : MonoBehaviour
{
    [Header("Dialog Objects")]
    [SerializeField] private GameObject textBG;
    [SerializeField] private GameObject curupira;
    [SerializeField] private GameObject boss;
    [SerializeField] private TMP_Text lineCurupira;
    [SerializeField] private TMP_Text lineBoss;

    [Header("Colliders")]
    [SerializeField] private DialogCollisionsHandler firstCollision;
    [SerializeField] private DialogCollisionsHandler secondCollision;

    [Header("Outside Verifiers")]
    [SerializeField] private BaseEnemyAI enemy;
    [SerializeField] private BaseEnemyAI enemyTwo;
    [SerializeField] private DeerReleaser firstPuzzle;
    [SerializeField] private SecondPuzzleVerifier secondPuzzle;
    [SerializeField] private TargetsOutlineHandler thirdPuzzle;
    [SerializeField] private TargetsPuzzleHandler thirdPuzzleHandler;
    [SerializeField] private BossEnemyAI bossArea;


    private bool firstTimeEnemies;
    private bool firstTimeFirstPuzzle;
    private bool firstTimeSecondPuzzle;
    private bool firstTimeThirdPuzzle;
    private bool secondTimeThirdPuzzle;
    private bool firstTimeBoss;
    private bool secondTimeBoss;

    private bool bossLine;

    private Coroutine routine;

    void Start()
    {
        firstTimeEnemies = false;
        firstTimeFirstPuzzle = false;
        firstTimeSecondPuzzle = false;
        firstTimeThirdPuzzle = false;
        secondTimeThirdPuzzle = false;
        firstTimeBoss = false;
        secondTimeBoss = false;
        bossLine = false;
        ShowDialog("Malditos cacadores... Olhem so o que fizeram com a minha casa! Isso nao vai ficar assim!", 1, 0);
    }

    void Update()
    {
        if (firstCollision.playerInside && !firstCollision.firsTime)
        {
            ShowDialog("Dois caminhos fechados e apenas um aberto... Tem algo errado aqui. Preciso investigar e ajudar quem estiver em perigo.", 1, 1);
            firstCollision.firsTime = true;
        }

        if ((enemy.playerInSightRange || enemyTwo.playerInSightRange) && !firstTimeEnemies)
        {
            ShowDialog("Eles acham mesmo que conseguem me parar com isso? Estao muito enganados!", 1, 0);
            firstTimeEnemies = true;
        }

        if (secondCollision.playerInside && !secondCollision.firsTime)
        {
            ShowDialog("Um desafio para libertar os animais? Quanta maldade..., mas eu nao vou deixa-los para tras.", 1, 2);
            secondCollision.firsTime = true;
        }

        if(firstPuzzle.secondPuzzleEnemies.activeSelf && !firstTimeFirstPuzzle)
        {
            ShowDialog("Um animal a salvo... E com isso, um dos caminhos se abriu. Ainda ha mais a fazer.", 1, 0);
            firstTimeFirstPuzzle = true;
        }

        if(secondPuzzle.attempts == 2 && !firstTimeSecondPuzzle)
        {
            ShowDialog("Hm... As flores parecem esconder a resposta. Preciso observar com atencao.", 1, 0);
            firstTimeSecondPuzzle = true;
        }

        if(thirdPuzzle.playerInside && !firstTimeThirdPuzzle)
        {
            ShowDialog("Querem testar a minha mira? Esses cacadores esqueceram com quem estao lidando!", 1, 0);
            firstTimeThirdPuzzle = true;
        }

        if (thirdPuzzleHandler.puzzleComplete && !secondTimeThirdPuzzle)
        {
            ShowDialog("Finalmente, o ultimo caminho esta livre. Agora eles vao sentir a furia da floresta!", 1, 0);
            secondTimeThirdPuzzle = true;
        }
        
        if(bossArea.playerInside && !firstTimeBoss) 
        {
            ShowDialog("Jorge! Voce passou dos limites. A natureza nao vai mais tolerar a sua ganancia!", 1, 0);
            textBG.transform.position = new Vector3(textBG.transform.position.x, textBG.transform.position.y-170, textBG.transform.position.z);
            firstTimeBoss = true;
        }

        if (bossLine && !secondTimeBoss) 
        {
            ShowDialog("Ha! Um espirito da floresta tentando me parar? Vamos ver se voce eh tao forte quanto pensa.", 2, 0);
            secondTimeBoss = true;
        }
    }

    private void ShowDialog(string message, int whatChar, int collider)
    {
        
        textBG.SetActive(true);
        if (whatChar == 1)
        {
            curupira.SetActive(true);
            lineCurupira.SetText(message);
        }

        else if(whatChar == 2)
        {
            boss.SetActive(true);
            lineBoss.SetText(message);
        }
        if (routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(HideDialogAfterDelay(8f, collider));
    }

    private IEnumerator HideDialogAfterDelay(float delay, int collider)
    {
        yield return new WaitForSeconds(delay);
        if (collider == 1)
            firstCollision.gameObject.SetActive(false);
        else if (collider == 2)
            secondCollision.gameObject.SetActive(false);
        textBG.SetActive(false);
        curupira.SetActive(false);
        boss.SetActive(false);
        if (firstTimeBoss)
        {
            yield return new WaitForSeconds(.5f);
            bossLine = true;
        }
    }
}