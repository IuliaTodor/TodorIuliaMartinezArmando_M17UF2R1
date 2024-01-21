using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    /// <summary>
    /// La lista de los prefabs que tiene el enemigo
    /// </summary>
    public List<GameObject> droppedItemPrefab = new List<GameObject>();
    /// <summary>
    /// La lista de los scriptable objects que tiene el enemigo
    /// </summary>
    public List<Item> lootList = new List<Item>();
    /// <summary>
    /// La lista de los índices de los objetos
    /// </summary>
    private List<int> droppedItemIndexList = new List<int>();
    /// <summary>
    /// El índice del objeto que ha droppeado el enemigo
    /// </summary>
    private int droppedItemIndex;

    /// <summary>
    /// Se determina el objeto que va a droppear el enemigo
    /// </summary>
    /// <returns></returns>
    public Item GetDropepdItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();

        for (int i = 0; i < lootList.Count; i++)
        {
            //Añadimos los objetos que hayan tocado de random
            if (randomNumber <= lootList[i].dropChance)
            {
                possibleItems.Add(lootList[i]);
                droppedItemIndexList.Add(i);
            }
        }

        //Si se han añadido items seleccionamos uno, así como su índice
        if (possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            for (int i = 0; i < droppedItemPrefab.Count; i++)
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

    /// <summary>
    /// Instancia el item que ha salido
    /// </summary>
    /// <param name="spawnPosition"></param>
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Item droppedItem = GetDropepdItem();
        if (droppedItem != null)
        {
            GameObject lootGO = Instantiate(droppedItemPrefab[droppedItemIndex], spawnPosition, Quaternion.identity);
            lootGO.GetComponent<SpriteRenderer>().sprite = droppedItem.image;
        }

        else
        {
            return;
        }

    }
}
