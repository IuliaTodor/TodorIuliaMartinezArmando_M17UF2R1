using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] public Vector3[] points;
    public Vector3 currentPosition { get; set; } //La posición actual del NPC
    private bool isGameInitialized;


    private void Start()
    {
        isGameInitialized = true;
        currentPosition = transform.position;
    }

    //Para saber la posición del punto al cual nos queremos mover
    public Vector3 GetMovementPosition(int index)
    {
        return currentPosition + points[index];
    }
    private void OnDrawGizmos()
    {
        //Mientras estemos en el editor y mientras estemos cambiando la posición del NPC en el editor, actualizamos la posición actual a la del transform
        //De esta forma la posición por default del primer waypoint (0,0,0) será la del NPC
        if (!isGameInitialized && transform.hasChanged) 
        {
            currentPosition = transform.position;
        }


        if(points == null || points.Length <= 0)
        {
            return;
        }

        for(int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(points[i] + currentPosition, 0.5f); //Dibujamos una esfera de 0.5 de radio para marcar donde están los puntos
            //Si no nos pasamos de la cantidad de puntos en el array
            if(i < points.Length - 1)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(points[i] + currentPosition, points[i+1] + currentPosition);
            }

        }
    }

}
