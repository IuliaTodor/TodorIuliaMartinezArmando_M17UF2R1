using System;
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
        instance = this;
    }

    void Start()
    {
        items = new Item[slotsNumber];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(Item itemToAdd, int quantity)
    {
        if (itemToAdd == null || quantity <= 0)
        {
            return;
        }

        List<int> indexCount = VerifyExistingItem(itemToAdd.ID);

        //Verificación en caso de estar cogiendo un item que ya tenemos
        if (itemToAdd.isStackable)
        {
            if (indexCount.Count > 0)
            {
                for (int i = 0; i < indexCount.Count; i++)
                {
                    //Cogemos el primer item que está en un slot.
                    if (items[indexCount[i]].amount < itemToAdd.maxAmount) //Si el item a añadir no ha superado su cantidad máxima lo añadimos
                    {
                        items[indexCount[i]].amount += quantity; //Añadimos la cantidad a ese item

                        //Si ha superado la cantidad máxima cogemos la cantidad por la cual la supera y añadimos el item, su cantidad será la diferencia
                        if (items[indexCount[i]].amount > itemToAdd.maxAmount)
                        {
                            int difference = items[indexCount[i]].amount - itemToAdd.maxAmount;
                            items[indexCount[i]].amount = itemToAdd.maxAmount;
                            AddItem(itemToAdd, difference);
                        }

                        InventoryUI.instance.DrawItemOnInventory(itemToAdd, items[indexCount[i]].amount, indexCount[i]);
                        return;
                    }
                }
            }
        }
        //Para explicar la siguiente parte digamos que conseguimos 20 de un objeto de curación (con un maxAmmount de 10)

        //Si el item no está en el item
        if (quantity > itemToAdd.maxAmount)
        {
            //Añadimos 50 en un nuevo slot
            AddItemOnNewSlot(itemToAdd, itemToAdd.maxAmount);
            //Actualizamos su cantidad (50-60)
            quantity -= itemToAdd.maxAmount;
            //Volvemos a llamar a la función para que maneje los 10 restantes
            AddItem(itemToAdd, quantity);
        }

        else
        {
            //Añadiría los 10 restantes en un nuevo slot
            AddItemOnNewSlot(itemToAdd, quantity);
        }

    }
    //Como hay un número fijo de slots, el ID del slot corresponderá al del objeto en ese slot

    //Verifica si en el inventario hay algún item con el ID del que vamos a añadir.
    //Si es así la función guarda los índex de los slot de los objetos que hay en el inventario en la lista
    //Así pues, en la lista tendríamos los índex de los dos slots con los objetos
    private List<int> VerifyExistingItem(string itemID)
    {
        List<int> itemIndex = new List<int>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null) //Comprobamos si en el slot existe un item
            {
                if (items[i].ID == itemID)
                {
                    itemIndex.Add(i);
                }
            }
        }

        return itemIndex;
    }

    //Añade objeto en un slot vacío
    private void AddItemOnNewSlot(Item item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //Si el slot está vacío añade el item
            if (items[i] == null)
            {
                items[i] = item.CopyItem(); //Así al añadir otro item no referencia al mismo SO
                items[i].amount = quantity;
                InventoryUI.instance.DrawItemOnInventory(item, quantity, i);
                return; //Para salir tras añadir los items
            }
        }
    }

    //Eliminamos de la cantidad del item, y si ya no hay lo quitamos del inventario
    private void DeleteItem(int index)
    {
        items[index].amount--;

        if (items[index].amount <= 0)
        {
            items[index].amount = 0;
            items[index] = null; //el item ya no existe porque no tiene cantidad
            InventoryUI.instance.DrawItemOnInventory(null, 0, index);
        }

        else
        {
            InventoryUI.instance.DrawItemOnInventory(items[index], items[index].amount, index); //Actualizamos la cantidad del item
        }
    }

    public void MoveItem(int initialIndex, int finalIndex)
    {
        //Comrpobamos si el slot inicial tiene un item y si el slot final no tiene ya un item
        if (items[initialIndex] == null || items[finalIndex] != null)
        {
            return;
        }

        //Copiar el item en el slot de finalIndex
        Item itemToMove = items[initialIndex].CopyItem();
        items[finalIndex] = itemToMove;
        InventoryUI.instance.DrawItemOnInventory(itemToMove, itemToMove.amount, finalIndex);

        //Borramos el item del slot inicial
        items[initialIndex] = null;
        InventoryUI.instance.DrawItemOnInventory(null, 0, initialIndex);
    }

    //Verificamos si se puede usar el item, y si es así eliminamos uno de su cantidad
    private void UseItem(int index)
    {
        if (items[index] == null)
        {
            return;
        }

        if (items[index].UseItem()) //Si el item se ha podido usar
        {
            DeleteItem(index);
        }
    }

    #region EventManager
    private void SlotInteractionResponse(interactionType type, int index)
    {
        switch (type)
        {
            case interactionType.Use:
                UseItem(index);
                break;
            case interactionType.Equip:
                break;
            case interactionType.Discard:
                break;

        }
    }


    private void OnEnable()
    {
        InventorySlot.eventSlotInteraction += SlotInteractionResponse;
    }


    private void OnDisable()
    {
        InventorySlot.eventSlotInteraction -= SlotInteractionResponse;
    }
    #endregion

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
