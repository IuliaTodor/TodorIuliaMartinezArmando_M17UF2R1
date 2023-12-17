using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void characterDeathResponse()
    {
        _animator.SetBool(GameManager.instance.characterDeath, true);

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

