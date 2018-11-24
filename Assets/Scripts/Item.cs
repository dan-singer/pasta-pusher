using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType{
	PPC,
	PPS,
	TempPPC
}

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
	public string Name;
	public string Description;
	public int Cost;
	public int Value;
	public float Duration;
	public ItemType Type;
}


