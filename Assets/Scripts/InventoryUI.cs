using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot slotPrefabs;
    [SerializeField] private Transform container;

    List<InventorySlot> slots = new List<InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        for(int i = 0; i < Inventory.instance.slotsNumber; i++)
        {
            
            InventorySlot newSlot = Instantiate(slotPrefabs, container);
            newSlot.index = i;  
            slots.Add(newSlot);
        }
    }
}
