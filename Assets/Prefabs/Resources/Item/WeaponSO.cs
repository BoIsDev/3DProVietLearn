using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "SO/WeaponSO")]

public class WeaponSO : ScriptableObject
{
    public string itemName;
    public ItemCode itemCode;
    public int dame = 1;
    public int level = 20;
    public Sprite itemImage;

}

