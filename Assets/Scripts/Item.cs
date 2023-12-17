using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Weapon,
    Treasures,
    CureItem
}
public class Item : ScriptableObject, IPickable
{
    public string ID;
    public string itemName;
    public Sprite image;
    [TextArea]public string description; //Para que se pueda escribir más
    public ItemTypes type;
    public bool isConsumible;
    public bool isStackable;
    public int maxAmount;

    [HideInInspector] public int amount;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
