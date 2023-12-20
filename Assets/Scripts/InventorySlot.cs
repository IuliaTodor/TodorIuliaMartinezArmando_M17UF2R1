using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Los tipos de interacción con el slot
public enum interactionType
{
    Click,
    Use,
    Equip,
    Discard
}

public class InventorySlot : MonoBehaviour
{
    public static Action<interactionType, int> eventSlotInteraction;

    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject amountBackground; //Para poder desactivarlo si no hay un item
    [SerializeField] private TextMeshProUGUI amountTMP;
    public int index { get; set; }

    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpdateSlot(Item item, int quantity)
    {
        itemIcon.sprite = item.image;
        amountTMP.text = quantity.ToString();
    }

    //Para activar y desactivar los slots
    public void ActivateSlotUI(bool state)
    {
        itemIcon.gameObject.SetActive(state);
        amountBackground.SetActive(state);
    }

    //Así el slot queda seleccionado cuando has usado un objeto
    public void SelectSlot()
    {
        button.Select();
    }

    //Lanzamos el evento de hacer click en el slot
    public void ClickSlot()
    {
        eventSlotInteraction?.Invoke(interactionType.Click, index);

        //Si tenemos un slot seleccionado (su valor es diferente al inicial)
        if(InventoryUI.instance.initalSlotIndexToMove != -1)
        {
            //Para saber si se puede mover el item al otro slot
            if(InventoryUI.instance.initalSlotIndexToMove != index)
            {
                Inventory.instance.MoveItem(InventoryUI.instance.initalSlotIndexToMove, index);
            }
        }
     }

    //Lanzamos el evento de usar el item del slot
    public void UseItemSlot()
    {
        //Comprobamos si hay un item en el slot
        if (Inventory.instance.items[index] != null)
        {
            //Si es así lanzamos el evento que indica que queremos usar el item
            eventSlotInteraction?.Invoke(interactionType.Use, index);
        }
    }

}
