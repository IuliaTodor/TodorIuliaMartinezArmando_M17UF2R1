using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla en qué dirección se está moviendo para definir su sprite
/// </summary>
public enum movementDirection
{
    Horizontal,
    Vertical,
    DiagonalLeft,
    DiagonalRight,
}

public class CharacterMovement : Character
{
    [SerializeField] private movementDirection direction;
    /// <summary>
    /// La posición de movimiento dependiendo del índice del waypoint
    /// </summary>
    public Vector3 pointToMove => waypoint.GetMovementPosition(currentPointIndex);
    private Vector3 lastPosition;

    private Waypoint waypoint;
    private Animator anim;
    /// <summary>
    /// Controla el índice del punto al que nos queremos mover
    /// </summary>
    private int currentPointIndex;

    void Start()
    {
        anim = GetComponent<Animator>();
        waypoint = GetComponent<Waypoint>();
        currentPointIndex = 0;
    }

    void Update()
    {
        MoveCharacter();
        ChangeCharacterAnimation();

        if (CheckCurrentPointReached())
        {
            UpdateIndexMovement();
        }
    }

    /// <summary>
    /// Mueve al personaje hacia el siguiente waypoint
    /// </summary>
    private void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointToMove, speed * Time.deltaTime);
    }

    /// <summary>
    /// Comprueba si ha llegado al siguiente waypoint
    /// </summary>
    /// <returns></returns>
    private bool CheckCurrentPointReached()
    {
        float distanceTowardsCurrentPoint = (transform.position - pointToMove).magnitude; //La magnitud es la longitud del vector
        if (distanceTowardsCurrentPoint < 0.1f)
        {
            lastPosition = transform.position;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Actualiza el índice del waypoint para que el personaje se mueva en bucle
    /// </summary>
    private void UpdateIndexMovement()
    {
        if (currentPointIndex == waypoint.points.Length - 1)
        {
            currentPointIndex = 0;
        }

        else
        {
            currentPointIndex++;
        }
    }

    /// <summary>
    /// Cambia la animación según el tipo de dirección
    /// </summary>
    private void ChangeCharacterAnimation()
    {
        switch (direction)
        {
            case movementDirection.Horizontal:
                HorizontalMovement();
                break;
            case movementDirection.Vertical:
                VerticalMovement();
                break;
            case movementDirection.DiagonalLeft:
                DiagonalLeftMovement();
                break;
            case movementDirection.DiagonalRight:
                DiagonalRightMovement();
                break;
            default:
                break;
        }
    }

    private void HorizontalMovement()
    {
        if (pointToMove.x > lastPosition.x)
        {
            anim.SetFloat("NPCXMovement", 1);
        }

        else
        {
            anim.SetFloat("NPCXMovement", -1);
        }
    }

    private void VerticalMovement()
    {
        if (pointToMove.y > lastPosition.y)
        {
            anim.SetFloat("NPCYMovement", 1);
        }

        else
        {
            anim.SetFloat("NPCYMovement", -1);
        }
    }

    private void DiagonalLeftMovement()
    {
        if (pointToMove.x > lastPosition.x)
        {
            anim.SetFloat("NPCXMovement", 1);
            anim.SetFloat("NPCYMovement", -1);

        }

        else
        {
            anim.SetFloat("NPCXMovement", -1);
            anim.SetFloat("NPCYMovement", 1);
        }
    }

    private void DiagonalRightMovement()
    {
        if (pointToMove.x > lastPosition.x)
        {
            anim.SetFloat("NPCXMovement", 1);
            anim.SetFloat("NPCYMovement", 1);

        }

        else
        {
            anim.SetFloat("NPCXMovement", -1);
            anim.SetFloat("NPCYMovement", -1);
        }
    }

}
