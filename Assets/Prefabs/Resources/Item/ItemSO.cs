using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public ItemCode itemCode;
    public int exp;
    public int quantity;
    public Sprite itemImage;

}
