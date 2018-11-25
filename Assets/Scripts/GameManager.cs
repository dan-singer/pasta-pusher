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

	/// <summary>
	/// Invoked when pps causes pasta to go up
	/// </summary>
	public event Action Ticked;
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
	private int ppc = 1;

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

	public int PPS{ get; private set; }

	public Item[] ShopItems;

	public event Action PowerupReceived, PowerupOver;


	private float timer = 0;
	void Start()
	{
	}

	void Update()
	{
		if (Time.time > timer + 1) {
			Pasta += PPS;
			timer = Time.time;
		}
	}

	public void ReceivePasta()
	{
		Pasta += PPC;
	}

	public void ReceiveUpgrade(Item item)
	{
		Pasta -= item.Cost;
		switch (item.Type){
			case ItemType.PPC:
				PPC = item.Value * PPC;
				break;
			case ItemType.PPS:
				PPS += item.Value;
				break;
			case ItemType.TempPPC:
				if (PowerupReceived != null)
					PowerupReceived();
				StartCoroutine(DelayResetPPC(item.Duration, PPC));
				PPC = item.Value * PPC;
				break;
		}
	}

	IEnumerator DelayResetPPC(float delay, int resetVal)
	{
		yield return new WaitForSeconds(delay);
		PPC = resetVal;
		if (PowerupOver != null)
			PowerupOver();
	}
}
