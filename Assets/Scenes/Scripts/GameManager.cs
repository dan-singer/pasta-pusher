using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main manager class for the game. Uses the Singleton design pattern.
/// </summary>
public class GameManager : MonoBehaviour {

	//Singleton pattern
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (!instance){
				instance = GameObject.FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	public event Action<int> PastaChanged;

	private int pasta;

	/// <summary>
	/// The current amount of pasta
	/// </summary>
	public int Pasta{
		get{
			return pasta;
		}
		private set{
			pasta = value;
			if (PastaChanged != null){
				PastaChanged(pasta);
			}
		}
	}


	public event Action<int> PPCChanged;
	private int ppc;

	/// <summary>
	/// Pasta-per-click
	/// </summary>
	public int PPC{ 
		get{
			return ppc;
		}
		private set{
			ppc = value;
			if (PPCChanged != null){
				PPCChanged(ppc);
			}
		} 
	}

	public Item[] ShopItems;

	public event Action<Sprite> PastaSpriteChanged;

	public void ReceivePasta()
	{
		Pasta += PPC;
	}

	public void ReceiveUpgrade(Item item)
	{
		PPC += item.Value;
		if (item is PastaItem){
			PastaItem pi = (PastaItem)item;
			if (PastaSpriteChanged != null){
				PastaSpriteChanged(pi.pastaSpr);
			}
		}
	}
}
