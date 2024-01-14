using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Bomb,
    Shooter,
    Tackle
}
public class AIManager : CharacterMovement
{
    [SerializeField] private AIState initialState;
    [SerializeField] private AIState currentState;
    [SerializeField] private AIState defaultState;

    [SerializeField] public float attackRange;
    [SerializeField] public float tackleRange;
    [SerializeField] public float detectionRange;

    [SerializeField] public float tackleSpeed;

    [SerializeField] public float enemyDamage;
    [SerializeField] public float enemyAttackCooldown; 
    
    [SerializeField] private float timeForNextAttack;
    private BoxCollider2D boxCollider2D;

    [SerializeField] public LayerMask characterLayerMask; 
    [SerializeField] public AttackType attackType;
    [HideInInspector] public Transform reference; //La referencia al player
    public CharacterMovement characterMovement;

    [SerializeField] private bool showDetection;
    [SerializeField] private bool showAttackRange;
    [SerializeField] private bool showTackleRange;

    //Si el tipo de ataque es tackle, el tipo de rango de ataque es tackleRange. Si no es AttackRange
    public float TypeOfAttackRange => attackType == AttackType.Tackle ? tackleRange : attackRange;


    private void Start()
    {
        boxCollider2D= GetComponent<BoxCollider2D>();
        currentState = initialState;
        characterMovement= GetComponent<CharacterMovement>();   
    }

    private void Update()
    {
        currentState.ExecuteState(this);
    }

    public void ChangeState(AIState newState)
    {
        if(newState != defaultState) 
        {
            currentState = newState;
        }
    }
    //Indica si el jugador está en rango de ataque del enemigo
    public bool AttackRange(float range)
    {
        float distanceTowardsPlayer = (reference.position - transform.position).sqrMagnitude; //La magnitud al cuadrado para comparar dos posiciones
        //Si está en el rango de ataque
        if (distanceTowardsPlayer < Mathf.Pow(range, 2))
        {
            return true;
        }
        return false;
    }

    public bool AttackTime()
    {
        if(Time.time > timeForNextAttack)
        {
            return true;
        }

        return false;
    }

    public void UpdateTimeBetweenAttacks()
    {
        timeForNextAttack= Time.time + enemyAttackCooldown; 
    }

    public void TackleAttack(float amount)
    {
        StartCoroutine(ITackle(amount));
    }

    private IEnumerator ITackle(float amount)
    {
        Vector3 playerPosition = reference.position;
        Vector3 enemyInitialPosition = transform.position;
        Vector3 directionTowardsPlayer = (playerPosition - enemyInitialPosition).normalized;
        Vector3 attackPosition = playerPosition - directionTowardsPlayer * 0.5f; //Para que el enemigo no entre del todo en contacto con el personaje
        boxCollider2D.enabled = false; //Desactivamos el BoxCollider del enemigo antes de que este ataque para evitar problemas

        float attackTransition = 0f;
        //Va de su posición inicial a la del jugador y luego de vuelta a la inicial
        while (attackTransition <= 1)
        {
            attackTransition += Time.deltaTime * speed;
            float interpolation = (-Mathf.Pow(attackTransition, 2) + attackTransition) * 4f;
            transform.position = Vector3.Lerp(enemyInitialPosition, attackPosition, interpolation);
            yield return null;
        }

        if (reference != null)
        {
            DamagePlayer(amount);
        }

        boxCollider2D.enabled = true;
    }

    public void DamagePlayer(float amount)
    {
        reference.GetComponent<Player>().HandleDamage(amount);
    }

    public void BombAttack(float amount)
    {
        if(reference != null)
        {
            DamagePlayer(amount);
        }
    }

    private void OnDrawGizmos()
    {
        if(showDetection)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }

        if (showAttackRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        if (showTackleRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, tackleRange);
        }
    }



}
