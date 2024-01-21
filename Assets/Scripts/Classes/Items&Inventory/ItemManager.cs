using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item itemInstance;
    [SerializeField] private int quantityToAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Inventory.instance.AddItem(itemInstance, quantityToAdd);
            Destroy(gameObject);
        }
    }


}
