using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Los tipos de interacción con el slot
/// </summary>
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

    /// <summary>
    /// Actualiza el Slot en función con su imagen en función del objeto
    /// </summary>
    /// <param name="item"></param>
    /// <param name="quantity"></param>
    public void UpdateSlot(Item item, int quantity)
    {
        itemIcon.sprite = item.image;
        amountTMP.text = quantity.ToString();
    }

    /// <summary>
    /// Activa y desactiva los slots
    /// </summary>
    /// <param name="state"></param>
    public void ActivateSlotUI(bool state)
    {
        itemIcon.gameObject.SetActive(state);
        amountBackground.SetActive(state);
    }

    /// <summary>
    /// Así el slot queda seleccionado cuando has usado un objeto
    /// </summary>
    public void SelectSlot()
    {
        button.Select();
    }

    /// <summary>
    /// Lanzamos el evento de hacer click en el slot
    /// </summary>
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
