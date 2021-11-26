using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "Inventory/new item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public int ItemType,TypeNum,ItemCost;
    public Sprite ItemIcon;
    [TextArea]
    public string Infornation;
}
