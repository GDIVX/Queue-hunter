using Combat;
using DG.Tweening;
using Game.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBarsManager : MonoBehaviour
{
    [SerializeField] float fillSpeed;
    [SerializeField] Image healthBar;
    [SerializeField] Image staminaBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        //healthBar.fillAmount = fillAmount;
        healthBar.DOFillAmount(fillAmount, fillSpeed);
    }

    public void UpdateStaminaBar(float cooldown)
    {
        staminaBar.fillAmount = 0;
        staminaBar.DOFillAmount(1, cooldown);
    }

}
