using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float playerHealth;
    public float currentHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthTxt;

    [Header("Regeneration Settings")]
    [SerializeField] private float timeBeforeRegen = 10f;
    [SerializeField] private float regenRate = 5f;
    [SerializeField] private float regenInterval = 0.1f;
    private float timeSinceLastDamage;
    private bool isRegenerating;
    public bool weaponInside;

    void Start()
    {
        currentHealth = playerHealth;
        timeSinceLastDamage = timeBeforeRegen;
    }

    void Update()
    {
        healthSlider.value = currentHealth / playerHealth;
        healthTxt.text = "" + Mathf.Round(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SceneManager.LoadScene(4);
            return;
        }

        if (currentHealth >= playerHealth)
        {
            timeSinceLastDamage = 0;
            isRegenerating = false;
            return;
        }

        timeSinceLastDamage += Time.deltaTime;
        if (timeSinceLastDamage >= timeBeforeRegen && !isRegenerating)
        {
            isRegenerating = true;
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (timeSinceLastDamage >= timeBeforeRegen && currentHealth < playerHealth)
        {
            currentHealth = Mathf.Min(currentHealth + (regenRate * regenInterval), playerHealth);

            yield return new WaitForSeconds(regenInterval);
        }

        isRegenerating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapons"))
        {
            weaponInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyWeapons"))
        {
            weaponInside = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        timeSinceLastDamage = 0;
        isRegenerating = false;
    }
}