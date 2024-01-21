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
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] public Image playerLife;
    [SerializeField] private Image playerAmmo;
    [SerializeField] private Image enemyLife;

    //[SerializeField] private TextMeshProUGUI playerLifeTMPro;
    [SerializeField] private TextMeshProUGUI playerAmmoTMPro;
    [SerializeField] private TextMeshProUGUI coinsTMP;

    public bool GameIsPaused = false;
    public float health;
    public float maxHealth;

    private float ammo;
    private float maxAmmo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        UpdateCharacterUI();

        if(!HandleEnemyHealth.instance.isDead)
        {
            UpdateEnemyUI();
        }
        TogglePauseMenu();
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

    private void UpdateEnemyUI()
    {
        //Mueve el fill amount entre 0 y 3
        enemyLife.fillAmount = Mathf.Lerp(enemyLife.fillAmount, health / maxHealth, 10f * Time.deltaTime);
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

    public void TogglePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                //FindObjectOfType<AudioManager>().Play("ClosePauseMenu");
            }

            else
            {
                Pause();

            }
        }
    }

    public IEnumerator GameOverMenu()
    {
        if (Player.instance.isDead)
        {
            yield return new WaitForSeconds(1);
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

        else
        {
            Time.timeScale = 1.0f;
            gameOverPanel.SetActive(false);
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //FindObjectOfType<AudioManager>().Play("ClosePauseMenu");
    }

    void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //FindObjectOfType<AudioManager>().Play("OpenPauseMenu");
    }

    #region Paneles

    public void OpenInventory()
    {
        Time.timeScale = 0f;
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void CloseInventory()
    {
        Time.timeScale = 1f;
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
