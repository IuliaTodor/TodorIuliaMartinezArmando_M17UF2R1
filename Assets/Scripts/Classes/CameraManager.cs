using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 maxPosition;
    [SerializeField] private Vector2 minPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        target = GameObject.FindWithTag("Player");
    }
    void Start()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }

    // El jugador se mueve en Update, mientras que la c�mara se mueve en Late Update, para que vaya despu�s del movimiento del jugador.
    //De esta forma el movimiento del jugador va antes que el de la c�mara
    void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.transform.position)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

                //Establece l�mites para los bordes de la c�mara
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                //Lerp (linear interpolation) calcula la distancia entre dos puntos y mueve el primer punto un porcentaje de esa distancia
                //cada frame hacia el segundo punto
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
            }
        }
    }
}
