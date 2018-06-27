using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
	[Header("Player Stats")]
	public float maxHealth = 100;
	public float maxMagic = 100;
	public float maxStamina = 100;
	public float staminaRegenRate = 1;

	[Header("UI Objects")]
	public Image healthBar;
	public Image magicBar;
	public Image staminaBar;

	private float currentHealth, currentMagic, currentStamina;

	private void Start()
	{
		currentHealth = maxHealth;
		currentMagic = maxMagic;
		currentStamina = maxStamina;
	}

	private void Update()
	{
		healthBar.fillAmount = currentHealth / maxHealth;
        magicBar.fillAmount = currentMagic / maxMagic;
        staminaBar.fillAmount = currentStamina / maxStamina;
	}

	public IEnumerator StaminaCooldownCoroutine()
	{
		yield return new WaitForSeconds(3f);
		StartCoroutine(StaminaRegeneration());
	}

	public IEnumerator StaminaRegeneration()
	{
		yield return new WaitForSeconds(staminaRegenRate);
		if (currentStamina < maxStamina)
		{
			currentStamina++;
			StartCoroutine(StaminaRegeneration());
		}
	}

	public void ChangeHealth(int health)
	{
		if (currentHealth + health <= maxHealth)
			currentHealth += health;
		else
			currentHealth = maxHealth;
	}

	public void ChangeMagic(int magic)
	{
        if (currentMagic + magic <= maxMagic)
            currentMagic += magic;
        else
            currentMagic = maxMagic;
	}

	public void ChangeStamina(int stamina)
	{
		if (stamina < 0)
		{
			currentStamina += stamina;
			StartCoroutine(StaminaCooldownCoroutine());
		}
		else if (currentStamina + stamina <= maxStamina)
			currentStamina += stamina;
		else if (currentStamina + stamina > maxStamina)
			currentStamina = maxStamina;
	}

	public void ChangeMaxHealth(int health)
	{
		currentHealth = (currentHealth / maxHealth) * (maxHealth + health);
		maxHealth += health;
	}

    public void ChangeMaxMagic(int magic)
    {
        currentMagic = (currentMagic / maxMagic) * (maxMagic + magic);
        maxMagic += magic;
    }

    public void ChangeMaxStamina(int stamina)
    {
        currentStamina = (currentStamina / maxStamina) * (maxStamina + stamina);
        maxStamina += stamina;
    }
}
