using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory;
    public Item itemEquipped;

    private Item foundItem;

    public void Update()
    {
        print(GetItemIndex("Raw Meat"));
    }

    public int GetItemIndex(string name)
    {
        foreach (Item items in inventory)
        {
            if (items.name == name)
            {
                foundItem = items;
                break;
            }
        }

        return inventory.IndexOf(foundItem);
    }

    public void Add(Item item, int amount)
    {
        if (inventory.Contains(item))
        {
            item.amount += amount;
        }
        else
        {
            inventory.Add(item);
            item.amount += amount;
        }
    }

    public void Remove(Item item, bool drop, Vector3 playerPos)
    {
        if (!inventory.Contains(item))
        {
            return;
        }
        else if (inventory.Contains(item) && drop)
        {
            inventory.Remove(item);
            Instantiate(item.assiociatedGameobject, playerPos, Quaternion.identity);
        }
        else if (inventory.Contains(item) && !drop)
        {
            inventory.Remove(item);
        }
    }
}
