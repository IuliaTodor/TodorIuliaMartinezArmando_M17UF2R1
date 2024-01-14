using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectilePosition;

    private float timer;
    [SerializeField] private float maxTime = 2;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > maxTime)
        {
            timer = 0;

            //As� no dispara si el jugador est� muriendo
            if (!Player.instance.isDead)
            {
                Shoot();
            }

        }
    }

    private void Shoot()
    {
        Instantiate(projectile, projectilePosition.position, Quaternion.identity);
    }
}
