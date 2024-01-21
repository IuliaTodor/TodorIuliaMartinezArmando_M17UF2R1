using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Item> lootList = new List<Item>();

    public Item GetDropepdItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();
        foreach (Item item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }

        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Item droppedItem = GetDropepdItem();
        if(droppedItem != null)
        {
            GameObject lootGO = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGO.GetComponent<SpriteRenderer>().sprite = droppedItem.image;
        }
    }
}
