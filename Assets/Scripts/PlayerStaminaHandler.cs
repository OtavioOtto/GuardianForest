using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStaminaHandler : MonoBehaviour
{
    [SerializeField] private float playerStamina = 1f;
    public float currentStamina;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private WaitForSeconds regenTick = new WaitForSeconds(.1f);
    [SerializeField] private PlayerMovement move;

    private Coroutine rechargeCoroutine;
    private bool isWaitingForCooldown = false;

    void Start()
    {
        currentStamina = playerStamina;
    }

    void Update()
    {
        staminaSlider.value = currentStamina / playerStamina;

        if (move.isSprinting)
        {
            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
                rechargeCoroutine = null;
            }
            isWaitingForCooldown = false;

            currentStamina -= 0.005f;
        }
        else if (!move.isSprinting && rechargeCoroutine == null && !isWaitingForCooldown)
        {
            rechargeCoroutine = StartCoroutine(StaminaRecharge());
        }
    }

    IEnumerator StaminaRecharge()
    {
        isWaitingForCooldown = true;
        yield return new WaitForSeconds(2f);
        isWaitingForCooldown = false;

        float startStamina = currentStamina;
        float targetStamina = playerStamina;
        float elapsedTime = 0f;

        while (elapsedTime < 2 && currentStamina < playerStamina)
        {
            elapsedTime += Time.deltaTime;
            float lerpProgress = elapsedTime / 2;
            currentStamina = Mathf.Lerp(startStamina, targetStamina, lerpProgress);
            yield return null;
        }

        rechargeCoroutine = null;
    }
}