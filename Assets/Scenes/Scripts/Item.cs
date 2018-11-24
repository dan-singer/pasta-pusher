using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
	public string Name;
	public string Description;
	public int Cost;
	public int Value;
}

[CreateAssetMenu(fileName = "PastaItem", menuName = "Inventory/PastaItem")]
public class PastaItem : Item {
	public Sprite pastaSpr;
}

