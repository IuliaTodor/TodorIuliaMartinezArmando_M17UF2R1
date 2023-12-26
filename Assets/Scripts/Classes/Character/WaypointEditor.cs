using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))] //Indicamos que es un editor custom para la clase Waypoint
public class WaypointEditor : Editor
{
    Waypoint WaypointTarget => target as Waypoint; //Así accedemos al contenido de la clase Waypoint

    [System.Obsolete]
    //Método llamado cuando hay cambios en la escena
    private void OnSceneGUI()
    {
        Handles.color = Color.white;
        if (WaypointTarget.points == null)
        {
            return;
        }

        for (int i = 0; i < WaypointTarget.points.Length; i++)
        {
            EditorGUI.BeginChangeCheck(); //Empieza a comprobar los cambios en el editor
            Vector3 currentPoint = WaypointTarget.currentPosition + WaypointTarget.points[i];
            //Creamos el Handle. Este es un control visual que permite modificar objetos dentro de la escena
            //Su posición es la del waypoint más la del NPC, sin rotación, de 0.7 de radio, con un tamaño 0.3 cuando es seleccionado y de tipo esfera
            Vector3 newPoint = Handles.FreeMoveHandle(currentPoint, Quaternion.identity, 0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

            //Creamos el texto al lado del Handle de movimiento, para indicar el índice de este
            GUIStyle text = new GUIStyle();
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 16;
            text.normal.textColor = Color.black;
            Vector3 alignment = Vector3.down * 0.3f + Vector3.right * 0.3f; //La alineación del texto respecto al handle de movimiento

            //Creamos el texto como un Handle de Label. Su posición es abajo a la derecha del Handle de movimiento, con el texto el índice y el estilo que le hemos puesto antes
            Handles.Label(WaypointTarget.currentPosition + WaypointTarget.points[i] + alignment, $"{i + 1}", text); //i+1 para que empiece desde 1 en vez de 0

            //Comprueba los cambios al finalizar
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle"); //Graba los cambios hechos en el editor por si se tiene que volver atrás
                WaypointTarget.points[i] = newPoint - WaypointTarget.currentPosition; //La posición del waypoint se actualiza en base a la del handle
            }
        }
    }
}
