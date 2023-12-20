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

        //Verificaci�n en caso de estar cogiendo un item que ya tenemos
        if (itemToAdd.isStackable)
        {
            if (indexCount.Count > 0)
            {
                for (int i = 0; i < indexCount.Count; i++)
                {
                    //Cogemos el primer item que est� en un slot.
                    if (items[indexCount[i]].amount < itemToAdd.maxAmount) //Si el item a a�adir no ha superado su cantidad m�xima lo a�adimos
                    {
                        items[indexCount[i]].amount += quantity; //A�adimos la cantidad a ese item

                        //Si ha superado la cantidad m�xima cogemos la cantidad por la cual la supera y a�adimos el item, su cantidad ser� la diferencia
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
        //Para explicar la siguiente parte digamos que conseguimos 20 de un objeto de curaci�n (con un maxAmmount de 10)

        //Si el item no est� en el item
        if (quantity > itemToAdd.maxAmount)
        {
            //A�adimos 50 en un nuevo slot
            AddItemOnNewSlot(itemToAdd, itemToAdd.maxAmount);
            //Actualizamos su cantidad (50-60)
            quantity -= itemToAdd.maxAmount;
            //Volvemos a llamar a la funci�n para que maneje los 10 restantes
            AddItem(itemToAdd, quantity);
        }

        else
        {
            //A�adir�a los 10 restantes en un nuevo slot
            AddItemOnNewSlot(itemToAdd, quantity);
        }

    }
    //Como hay un n�mero fijo de slots, el ID del slot corresponder� al del objeto en ese slot

    //Verifica si en el inventario hay alg�n item con el ID del que vamos a a�adir.
    //Si es as� la funci�n guarda los �ndex de los slot de los objetos que hay en el inventario en la lista
    //As� pues, en la lista tendr�amos los �ndex de los dos slots con los objetos
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

    //A�ade objeto en un slot vac�o
    private void AddItemOnNewSlot(Item item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //Si el slot est� vac�o a�ade el item
            if (items[i] == null)
            {
                items[i] = item.CopyItem(); //As� al a�adir otro item no referencia al mismo SO
                items[i].amount = quantity;
                InventoryUI.instance.DrawItemOnInventory(item, quantity, i);
                return; //Para salir tras a�adir los items
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

    //Verificamos si se puede usar el item, y si es as� eliminamos uno de su cantidad
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
