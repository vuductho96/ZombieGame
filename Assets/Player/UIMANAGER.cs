using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMANAGER2 : MonoBehaviour
{
    public Slider runStamina;
    public float maxStamina = 100f;
    private bool isRunning;

    private float savedStamina;

    void Start()
    {
        savedStamina = maxStamina;
    }

    public void AutoReloadStamina()
    {
        StartCoroutine(ReloadStaminaAfterDelay(4f));
    }

    private IEnumerator ReloadStaminaAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isRunning = false;
        UpdateStaminaBar();
    }

    private void CheckRunnable()
    {
        if (runStamina.value > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
            if (runStamina.value <= 0)
            {
                savedStamina = runStamina.value;
                AutoReloadStamina();
            }
        }
    }

    private void UpdateStaminaBar()
    {
        runStamina.value = isRunning ? runStamina.value - Time.deltaTime : maxStamina;
    }

    void Update()
    {
        CheckRunnable();
        UpdateStaminaBar();
    }
}
