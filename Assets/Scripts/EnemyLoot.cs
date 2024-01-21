using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public List<GameObject> droppedItemPrefab = new List<GameObject>();
    public List<Item> lootList = new List<Item>();
    private List<int> droppedItemIndexList = new List<int>();
    private int droppedItemIndex;

    public Item GetDropepdItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();

        for (int i = 0; i < lootList.Count; i++)
        {
            if (randomNumber <= lootList[i].dropChance)
            {
                possibleItems.Add(lootList[i]);
                droppedItemIndexList.Add(i);
            }
        }

        if (possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            for(int i = 0; i < droppedItemPrefab.Count; i++)
            {
                if (droppedItem.ID == droppedItemPrefab[i].name)
                {
                    droppedItemIndex = i;
                }
            } 

            return droppedItem;
        }



        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {   
        Item droppedItem = GetDropepdItem();
        if(droppedItem != null)
        {
            Debug.Log(droppedItemIndex);
            Debug.Log(droppedItem);
            Debug.Log(droppedItemPrefab[droppedItemIndex]);
            GameObject lootGO = Instantiate(droppedItemPrefab[droppedItemIndex], spawnPosition, Quaternion.identity);
            lootGO.GetComponent<SpriteRenderer>().sprite = droppedItem.image;
        }

        else
        {
            return;
        }
   
    }
}
