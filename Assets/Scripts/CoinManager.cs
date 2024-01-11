using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int totalCoins;

    void Start()
    {
        instance = this;
        totalCoins = 0;
    }

    void Update()
    {

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
