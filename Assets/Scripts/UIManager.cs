using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private Image playerLife;
    [SerializeField] private Image playerAmmo;

    //[SerializeField] private TextMeshProUGUI playerLifeTMPro;
    [SerializeField] private TextMeshProUGUI playerAmmoTMPro;

    private float health;
    private float maxHealth;

    private float ammo;
    private float maxAmmo;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void OpenCloseInventory()
    {
        //Pone el active al contrario de cómo está actualmente
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    #endregion
}
