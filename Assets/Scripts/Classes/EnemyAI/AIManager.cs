using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Los tipos de ataques de enemigos que habrá
/// </summary>
public enum AttackType
{
    Bomb,
    Projectile,
    Tackle
}
/// <summary>
/// Controla el comportamiento de los enemigos
/// </summary>
public class AIManager : CharacterMovement
{
    //---------------------Enemigo bomba------------------
    /**
     * El enemigo bomb debería tener: 
     * -Initial, current y defaultState
     * -Bomb range
     * -Detection range
     * -Enemy damage
     * -Explode anim
     * -showDetection
     * -showBombRange
     * */
    //---------------------Enemigo tackle------------------
    /**
     * El enemigo tackle debería tener: 
     * -Initial, current y defaultState
     * -Tackle range
     * -Detection range
     * -Tackle speed
     * -Enemyattackcooldown
     * -Timefornextattack
     * -Enemy damage
     * -showDetection
     * -showBombRange
     * */
    //---------------------Enemigo projectile------------------
    /**
   * El enemigo projectile debería tener: 
   * -Initial, current y defaultState
   * -Projectile range
   * -Detection range
   * -Enemyattackcooldown
   * -Timefornextattack
   * -Enemy damage
   * -Projectile
   * -Projectileposition
   * -showDetection
   * -showProjectileRange
   * */
    public static AIManager instance;
    //----------------------------------------------Estados------------------------------------------------------
    [SerializeField] private AIState initialState;
    [SerializeField] private AIState currentState;
    [SerializeField] private AIState defaultState;

    //----------------------------------------------Rango del enemigos--------------------------------------------
    //Estos tres son rangos en los cuales el enemigo va a atacar dependiendo de su tipo de ataque
    [SerializeField] public float bombRange;
    [SerializeField] public float tackleRange;
    [SerializeField] public float projectileRange;
    /// <summary>
    /// El rango en el cual el enemigo ve al jugador
    /// </summary>
    [SerializeField] public float detectionRange;
    //----------------------------------------------Caracteristicas específicas de enemigos------------------------
    /// <summary>
    /// Velocidad de ataque el enemigo tackle
    /// </summary>
    [SerializeField] public float tackleSpeed;
    [SerializeField] public float enemyAttackCooldown;
    /// <summary>
    /// Comprueba si puede atacar basado en el cooldown
    /// </summary>
    [SerializeField] private float timeForNextAttack;
    /// <summary>
    /// Daño que hace el enemigo
    /// </summary>
    [SerializeField] public float enemyDamage;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectilePosition;

    //----------------------------------------------Componentes------------------------
    private BoxCollider2D boxCollider2D;
    private Animator anim;
    [SerializeField] AnimationClip explodeAnim;  
    /// <summary>
    /// La referencia al player
    /// </summary>
    [HideInInspector] public Transform reference;   
    [HideInInspector] public CharacterMovement characterMovement;

    //----------------------------------------------Cosas color lima------------------------
    /// <summary>
    /// Layer mask del player
    /// </summary>
    [SerializeField] public LayerMask characterLayerMask;
    [SerializeField] public AttackType attackType;

    //----------------------------------------------Gizmos bool------------------------
    [SerializeField] private bool showDetection;
    [SerializeField] private bool showBombRange;
    [SerializeField] private bool showTackleRange;
    [SerializeField] private bool showProjectileRange;

    /// <summary>
    /// Define el rango de ataque usado según el tipo de este
    /// </summary>
    /// <returns></returns>
    public float TypeOfAttackRange()
    {
        switch (attackType)
        {
            case (AttackType.Tackle):
                return tackleRange;
            case (AttackType.Bomb):
                return bombRange;
            case (AttackType.Projectile):
                return projectileRange;
            default:
                return tackleRange;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        currentState = initialState;
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        currentState.ExecuteState(this);
    }

    /// <summary>
    /// Cambia el estado si el nuevo es diferente al actual
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(AIState newState)
    {
        if (newState != defaultState)
        {
            currentState = newState;
        }
    }
    /// <summary>
    /// Indica si el jugador está en rango de ataque del enemigo
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool AttackRange(float range)
    {
        float distanceTowardsPlayer = (reference.position - transform.position).sqrMagnitude; //La magnitud al cuadrado para comparar dos posiciones
        //Determina si el jugador está en el rango de ataque
        if (distanceTowardsPlayer < Mathf.Pow(range, 2))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Determina si el enemigo puede atacar tras acabar su cooldown
    /// </summary>
    /// <returns></returns>
    public bool IsAttackTime()
    {
        if (Time.time > timeForNextAttack)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Actualiza el tiempo de ataque basado en el cooldown
    /// </summary>
    public void UpdateTimeBetweenAttacks()
    {
        timeForNextAttack = Time.time + enemyAttackCooldown;
    }

    public void TackleAttack(float amount)
    {
        StartCoroutine(ITackle(amount));
    }

    public void BombAttack(float amount)
    {
        StartCoroutine(IBomb(amount));
    }
    public void ProjectileAttack()
    {
        StartCoroutine(IProjectile());
    }

    /// <summary>
    /// El enemigo tackle va de delante hacia atrás para atacar al jugador
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator ITackle(float amount)
    {
        Vector3 playerPosition = reference.position;
        Vector3 enemyInitialPosition = transform.position;
        Vector3 directionTowardsPlayer = (playerPosition - enemyInitialPosition).normalized;
        //Para que el enemigo no entre del todo en contacto con el personaje
        Vector3 attackPosition = playerPosition - directionTowardsPlayer * 0.5f;
        //Desactivamos el BoxCollider del enemigo antes de que este ataque para evitar problemas
        boxCollider2D.enabled = false; 

        float attackTransition = 0f;
        //Va de su posición inicial a la del jugador y luego de vuelta a la inicial
        while (attackTransition <= 1)
        {
            attackTransition += Time.deltaTime * speed;
            float interpolation = (-Mathf.Pow(attackTransition, 2) + attackTransition) * 4f;
            transform.position = Vector3.Lerp(enemyInitialPosition, attackPosition, interpolation);
            FindObjectOfType<AudioManager>().Play("EnemyTackle");
            yield return null;
        }

        if (reference != null)
        {
            DamagePlayer(amount);
        }

        boxCollider2D.enabled = true;
    }

    /// <summary>
    /// El enemigo bomba explota y hace daño al jugador
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator IBomb(float amount)
    {
        anim.SetBool("hasExploded", true);

        yield return new WaitForSeconds(explodeAnim.length);
        FindObjectOfType<AudioManager>().Play("EnemyExplosion");
        DamagePlayer(amount);

        Destroy(gameObject);
    }

    /// <summary>
    /// El enemigo proyectil dispara
    /// </summary>
    /// <returns></returns>
    private IEnumerator IProjectile()
    {
        Instantiate(projectile, projectilePosition.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("VaporeonProjectile");
        yield return true;
    }

    /// <summary>
    /// Hace daño al jugador
    /// </summary>
    /// <param name="amount"></param>
    public void DamagePlayer(float amount)
    {
        if(reference == null)
        {
            return;
        }

        reference.GetComponent<Player>().HandleDamage(amount);
    }

    private void OnDrawGizmos()
    {
        if (showDetection)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }

        if (showBombRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, bombRange);
        }

        if (showTackleRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, tackleRange);
        }

        if (showProjectileRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, projectileRange);
        }
    }



}
