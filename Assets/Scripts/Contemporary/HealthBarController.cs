using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Image healthFill; // Assign in Inspector
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject serverObject;   // Drag the Server GameObject in Inspector
    public GameObject winningScreen;
    public GameObject minigame1;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            StartCoroutine(FallServer());
        }
    }

    void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;
        healthFill.fillAmount = fillAmount;
    }

    private IEnumerator FallServer()
    {
        // when the server's HP is 0 there will be an animation that rotates the object by 90 degrees
        Quaternion start = serverObject.transform.rotation;
        Quaternion end = Quaternion.Euler(0, 0, 90);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime;
            serverObject.transform.rotation = Quaternion.Lerp(start, end, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        minigame1.SetActive(false);
        winningScreen.SetActive(true);
    }
}
