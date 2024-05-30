using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public float energyCost = 1f;
    public Organism organism;

    public float maxStamina = 100f;
    public float currentStamina;
    // Start is called before the first frame update
    public void Run(float energyCostForOrganism)
    {
        energyCost = energyCostForOrganism;
        currentStamina = maxStamina;
        StartCoroutine(UseStamina());
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += 25;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }
    IEnumerator UseStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (currentStamina > 0)
            {
                currentStamina -= energyCost;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
            else if(currentStamina <= 0) 
            {
                organism.Die();
            }
        }
    }

}
