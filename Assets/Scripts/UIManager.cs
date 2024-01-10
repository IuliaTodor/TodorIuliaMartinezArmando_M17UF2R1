using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject shopPanel;

    [SerializeField] private Image playerLife;
    [SerializeField] private Image playerAmmo;

    //[SerializeField] private TextMeshProUGUI playerLifeTMPro;
    [SerializeField] private TextMeshProUGUI playerAmmoTMPro;
    [SerializeField] private TextMeshProUGUI coinsTMP;

    private float health;
    private float maxHealth;

    private float ammo;
    private float maxAmmo;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateCharacterUI();
    }

    private void UpdateCharacterUI()
    {
        //Mueve el fill amount entre 0 y 3
        playerLife.fillAmount = Mathf.Lerp(playerLife.fillAmount, health/maxHealth, 10f * Time.deltaTime);

        //playerLifeTMPro.text = $"{health}/{maxHealth}";

        playerAmmo.fillAmount = Mathf.Lerp(playerAmmo.fillAmount, ammo/maxAmmo, 10f * Time.deltaTime);
        playerAmmoTMPro.text = $"{ammo}/{maxAmmo}";
        coinsTMP.text = CoinManager.instance.totalCoins.ToString();
    }

    public void UpdateCharacterHealth(float playerHealth, float playerMaxHealth)
    {
        //De esta forma los valores iniciales no son null
        health = playerHealth;
        maxHealth = playerMaxHealth;
    }

    public void UpdateCharacterAmmo(float playerAmmo, float playerMaxAmmo)
    {
        //De esta forma los valores iniciales no son null
        ammo = playerAmmo;
        maxAmmo = playerMaxAmmo;
    }

    #region Paneles

    public void ToggleInventory()
    {
        //Pone el active al contrario de c�mo est� actualmente
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void ToggleShop()
    {
        //Pone el active al contrario de c�mo est� actualmente
        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void OpenInteracitonPanel(extraNPCInteraction interactionType)
    {
        if(interactionType == extraNPCInteraction.Shop)
        {
            ToggleShop();
            DialogueManager.instance.ToggleDialogue(false);
        }
    }

    #endregion
}
