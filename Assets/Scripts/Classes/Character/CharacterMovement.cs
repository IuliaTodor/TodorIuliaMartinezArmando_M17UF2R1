using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Waypoint waypoint;
    private int currentPointIndex; //Para controlar el índice del punto al que nos queremos mover
    private Animator anim;
    public Vector3 pointToMove => waypoint.GetMovementPosition(currentPointIndex);
    private Vector3 lastPosition;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        waypoint = GetComponent<Waypoint>();
        sr = GetComponent<SpriteRenderer>();
        currentPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        ChangeCharacterAnimation();
        if (CheckCurrentPointReached())
        {
            UpdateIndexMovement();
        }
    }

    private void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointToMove, speed * Time.deltaTime);
    }

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

    private void UpdateIndexMovement()
    {
        //Para que el personaje se mueva en bucle
        if (currentPointIndex == waypoint.points.Length - 1)
        {
            currentPointIndex = 0;
        }

        else
        {
            currentPointIndex++;
        }
    }

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
