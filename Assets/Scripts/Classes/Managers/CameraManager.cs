using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private GameObject target;

    [SerializeField] private float speed;
    //Controlan los límites de la cámara
    [SerializeField] private Vector2 maxPosition;
    [SerializeField] private Vector2 minPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        target = GameObject.FindWithTag("Player");
    }
    void Start()
    {
        if (target != null)
        {
            //La posición inicial es la misma que la del jugador
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }

    // El jugador se mueve en Update, mientras que la cámara se mueve en Late Update, para que vaya después del movimiento del jugador.
    //De esta forma el movimiento del jugador va antes que el de la cámara
    void LateUpdate()
    {
        if (target != null)
        {
            //Si la posición de la cámara no es igual a la del jugador, se mueve suavemente hacia este usando Lerp
            if (transform.position != target.transform.position)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

                //Clamp establece el límite entre dos puntos. Lo usamos para establecer los límites de la cámara
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                //Lerp (linear interpolation) calcula la distancia entre dos puntos y mueve el primer punto
                //un porcentaje de esa distancia cada frame hacia el segundo punto
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
            }
        }
    }
}
