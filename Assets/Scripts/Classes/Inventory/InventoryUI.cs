using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private GameObject inventoryPanelDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private InventorySlot slotPrefabs;
    [SerializeField] private Transform container;

    //El index del slot que escogemos para mover
    public int initalSlotIndexToMove { get; private set; }
    public InventorySlot selectedSlot { get; private set; } //El slot que tengamos seleccionado
    List<InventorySlot> slots = new List<InventorySlot>();
    public static InventoryUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        InitializeInventory();
        initalSlotIndexToMove = -1;
    }

    private void Update()
    {
        UpdateSelectedSlot();
        if(Input.GetKeyUp(KeyCode.M))
        {
            if(selectedSlot != null)
            {
                initalSlotIndexToMove = selectedSlot.index; //Asignamos el índice del slot que estamos seleccionando
            }
        }
    }
    private void InitializeInventory()
    {
        for (int i = 0; i < Inventory.instance.slotsNumber; i++)
        {

            InventorySlot newSlot = Instantiate(slotPrefabs, container);
            newSlot.index = i;
            slots.Add(newSlot);
        }
    }

    //Actualizamos el slot seleccionado
    private void UpdateSelectedSlot()
    {
        GameObject GOSelected = EventSystem.current.currentSelectedGameObject; //El game object seleccionado

        //Si es null no hacemos nada
        if (GOSelected == null)
        {
            return;
        }

        InventorySlot slot = GOSelected.GetComponent<InventorySlot>(); //La referencia del slot seleccionado se guarda en la variable

        //SelectedSlot pasa a ser el slot que estamos seleccionando
        if (slot != null)
        {
            selectedSlot = slot;
        }

    }

    public void DrawItemOnInventory(Item itemToAdd, int quantity, int itemIndex)
    {
        InventorySlot slot = slots[itemIndex];
        if (itemToAdd != null)
        {
            slot.ActivateSlotUI(true);
            slot.UpdateSlot(itemToAdd, quantity);
        }

        else
        {
            slot.ActivateSlotUI(false);
        }
    }

    private void UpdateInventoryDescription(int index)
    {
        if (Inventory.instance.items[index] != null)
        {
            itemIcon.sprite = Inventory.instance.items[index].image;
            itemName.text = Inventory.instance.items[index].itemName;
            Debug.Log(itemName.text);
            itemDescription.text = Inventory.instance.items[index].description;

            inventoryPanelDescription.SetActive(true);
        }

        else
        {
            inventoryPanelDescription.SetActive(false);
        }
    }

    //Método que llamamos al usar el botón de Usar en el inventario
    public void UseItem()
    {
        //Si hay un objeto en ese slot, usamos su item
        if(selectedSlot !=null)
        {
            selectedSlot.UseItemSlot();
            selectedSlot.SelectSlot();
        }
    }

    #region EventManager
    private void SlotInteractionResponse(interactionType type, int index)
    {
        if (type == interactionType.Click)
        {
            UpdateInventoryDescription(index);
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
}
