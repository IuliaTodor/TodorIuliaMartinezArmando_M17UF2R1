using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int totalCoins;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        totalCoins = 0;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
    }

    public void RemoveCoins(int amount)
    {
        if (amount > totalCoins)
        {
            return;
        }
        
        totalCoins -= amount;

    }
}
