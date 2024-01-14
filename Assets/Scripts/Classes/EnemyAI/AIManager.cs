using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Bomb,
    Shooter
}
public class AIManager : MonoBehaviour
{
    [SerializeField] private AIState initialState;
    [SerializeField] private AIState currentState;
    [SerializeField] private AIState defaultState;
    public CharacterMovement characterMovement;

    [SerializeField] public float detectionRange;
    [SerializeField] public float movementSpeed;
    [SerializeField] public LayerMask characterLayerMask;
    [HideInInspector]public Transform reference; //La referencia al player

    [SerializeField] public float enemyDamage;
    [SerializeField] public AttackType attackType;
    [SerializeField] public float enemyAttackCooldown;
    [SerializeField] public float attackRange;
    [SerializeField] private float timeForNextAttack;

    [SerializeField] private bool showDetection;
    [SerializeField] private bool showAttackRange;


    private void Start()
    {
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
    }



}
