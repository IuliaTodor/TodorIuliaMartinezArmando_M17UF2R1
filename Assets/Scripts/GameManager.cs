using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Transforma en una variable con un id los parámetros del Animator
    [DoNotSerialize]public int characterDeath = Animator.StringToHash("isDead");

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameManager.instance.IsUnityNull());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
