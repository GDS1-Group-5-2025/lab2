using System;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] private string itemName;
    public string GetItemName(){ return itemName; }
    public void SetItemName(string newName){ itemName = newName; }
}
