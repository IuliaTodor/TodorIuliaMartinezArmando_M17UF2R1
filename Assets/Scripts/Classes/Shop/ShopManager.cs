using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Item[] items;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO; //La referencia al prefab en lugar del script
    public Button[] purchaseBtn;
    public Button[] increaseButton;
    public Button[] decreaseButton;
    public int[] itemAmount;

    // Start is called before the first frame update
    void Start()
    {
        //Para que solo aparezcan los que tienen algún Power Up asignado
        for (int i = 0; i < items.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
            shopPanels[i].amountTMP.text = "0";
            itemAmount[i] = int.Parse(shopPanels[i].amountTMP.text);
            Debug.Log(itemAmount);
        }
        LoadItems();
        CheckPurchaseable();
    }

    private void Update()
    {
        CheckPurchaseable();
    }
    public void LoadItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            shopPanels[i].itemNameTMP.text = items[i].name;
            shopPanels[i].costTMP.text = items[i].cost.ToString();
            shopPanels[i].itemImage.sprite = items[i].image;
            shopPanels[i].descriptionTMP.text = items[i].description;
        }
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (CoinManager.instance.totalCoins >= (items[i].cost * itemAmount[i]))
                {
                    purchaseBtn[i].interactable = true;
                }
                else
                {
                    purchaseBtn[i].interactable = false;
                }
        }
    }

    public void PurchaseItem(int btnNum)
    {
        if (CoinManager.instance.totalCoins >= items[btnNum].cost)
        {
            CoinManager.instance.totalCoins -= items[btnNum].cost * itemAmount[btnNum];
            CheckPurchaseable();

            Inventory.instance.AddItem(items[btnNum], itemAmount[btnNum]);

        }
    }

    public void IncreaseItemAmount(int btnNum)
    {
        itemAmount[btnNum]++;
        shopPanels[btnNum].amountTMP.text = itemAmount[btnNum].ToString();

    }

    public void DecreaseAmount(int btnNum)
    {
        if(itemAmount[btnNum] <= 0)
        {
            return;
        }

        itemAmount[btnNum]--;
        shopPanels[btnNum].amountTMP.text = itemAmount[btnNum].ToString();
    }
}
