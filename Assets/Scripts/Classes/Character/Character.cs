using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IMovement
{
    [SerializeField]
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    [SerializeField]
    private float _speed;


    void Start()
    {

    }

    void Update()
    {

    }
}
