using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public GameObject assiociatedGameobject; 
    public Sprite icon;

    public string itemName;
    public string description;

    public string onInteract;

    public float nutrition;
    public float damage;

    [Range(0, 420)]
    public int amount;

    private void Awake()
    {
        if (onInteract != "nothing" || onInteract != "eat" || onInteract != "attack" || onInteract != "shoot")
        {
            onInteract = "nothing";
        }
    }
}
