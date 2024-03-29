using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja la respuesta del evento de muerte del personaje
/// </summary>
public class HandleAnimations : MonoBehaviour
{
    private Animator _animator;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    private void characterDeathResponse()
    {
        _animator.SetBool(GameManager.instance.characterDeath, true);
        Player.instance.EnableLayer(Player.instance.idleLayer);
    }

    private void OnEnable()
    {
        Player.characterDeathEvent += characterDeathResponse;
    }

    private void OnDisable()
    {
        Player.characterDeathEvent -= characterDeathResponse;
    }


}

