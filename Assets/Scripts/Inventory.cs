using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    [SerializeField] public int slotsNumber;
    public static Inventory instance;

    private void Awake()
    {
        items = new Item[slotsNumber];
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {

    }


    public void CloseInventory()
    {

    }


    public void SelectItem()
    {

    }
}
