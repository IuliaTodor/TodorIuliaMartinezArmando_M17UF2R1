using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleEnemyHealth : MonoBehaviour, IHealth, IDamage
{
    public static HandleEnemyHealth instance;

    [SerializeField] private Image enemyLife;

    [SerializeField] private float _health;
    public float health
    {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private float _maxHealth;
    public float maxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    public bool isDead { get; set; }

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isDead = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        UIManager.instance.UpdateEnemyHealth(health, maxHealth);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    void Update()
    {
        if (!isDead)
        {
            UpdateEnemyUI();
        }
    }

    public void UpdateEnemyUI()
    {
        enemyLife.fillAmount = Mathf.Lerp(enemyLife.fillAmount, health / maxHealth, 10f * Time.deltaTime);
    }

    public void HandleDeath()
    {
        isDead = true;
        GetComponent<EnemyLoot>().InstantiateLoot(transform.position);
        boxCollider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject);
    }

    public void HandleDamage(float damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            UIManager.instance.UpdateEnemyHealth(health, maxHealth);

            if (health <= 0)
            {
                health = 0;
                UIManager.instance.UpdateEnemyHealth(health, maxHealth);
                HandleDeath();
            }
        }
    }


}
