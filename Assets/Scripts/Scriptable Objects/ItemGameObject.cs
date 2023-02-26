using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameObject : MonoBehaviour
{
    public Item referencedFrom;

    public Inventory inventory;

    public int amount;

    public bool canPickup = false;

    public void Awake()
    {
        amount = referencedFrom.amount;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        Invoke("removeCooldown", 3);
    }

    public void removeCooldown()
    {
        canPickup = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canPickup)
        {
            inventory.Add(referencedFrom, amount);
            Destroy(gameObject);
        }
    }
}
