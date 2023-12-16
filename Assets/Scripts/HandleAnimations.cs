using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimations : MonoBehaviour
{
    private Animator _animator;

    //Transforma en una variable con un id los parámetros del Animator
    private int characterDeath = Animator.StringToHash("isDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetBool(characterDeath, false);

        }
        
    }

    private void characterDeathResponse()
    {
        _animator.SetBool(characterDeath, true);

        Debug.Log("rip");
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

