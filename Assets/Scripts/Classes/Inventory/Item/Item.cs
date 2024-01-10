using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Weapon,
    Treasures,
    CureItem
}
public class Item : ScriptableObject
{
    public string ID;
    public string itemName;
    public Sprite image;
    [TextArea] public string description; //Para que se pueda escribir más
    public ItemTypes type;
    public bool isConsumible;
    public bool isStackable;
    public int maxAmount;
    public int amount;
    public int cost;

    //Crea una nueva instancia del objeto
    public Item CopyItem()
    {
        Item newInstance = Instantiate(this);
        return newInstance;
    }

    public virtual bool UseItem()
    {
        return true;
    }

    public virtual bool EquipItem()
    {
        return true;
    }

    public virtual bool DiscardItem()
    {
        return true;
    }


}
