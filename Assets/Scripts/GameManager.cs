using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main manager class for the game. Uses the Singleton design pattern. The Model of the Application.
/// </summary>
public class GameManager : MonoBehaviour {

    public enum State
    {
        Main = 0,
        Shop,
        Leaderboard
    }
    public State state { get; private set; }
    public event Action<State, State> StateChanged;

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

    // Number of pasta owned
    private int pasta;

    // Invoked when pasta quantity has changed
    public event Action<int> PastaChanged;

    // Invoked when a new item as become available
    public event Action<Item> ItemBecameAvailable;


    /// <summary>
    /// The current amount of pasta
    /// </summary>
    public int Pasta{
		get{
			return pasta;
		}
		private set{
            for (int i = 0; i < ShopItems.Length; ++i)
            {
                if (ItemBecameAvailable != null && pasta < ShopItems[i].Cost && value >= ShopItems[i].Cost)
                {
                    ItemBecameAvailable(ShopItems[i]);
                }
            }
            pasta = value;
			if (PastaChanged != null){
				PastaChanged(pasta);
			}
		}
	}


	public event Action<int> PPCChanged;


    /// <summary>
    /// Pasta Per Click
    /// </summary>
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
	private bool isPaused = false;
	void Start()
	{
		Pasta = 0;
	}

	void Update()
	{
		if (isPaused)
			return;
		if (Time.time > timer + 1) {
			Pasta += PPS;
			timer = Time.time;
		}
	}

    /// <summary>
    /// Pauses the clock. No Pasta will be accumulated anymore.
    /// </summary>
	public void Pause()
	{
		isPaused = true;
		Time.timeScale = 0;
	}

    /// <summary>
    /// Resumes the clock so pasta can continue to be devoured.
    /// </summary>
	public void Resume()
	{
		isPaused = false;
		Time.timeScale = 1;
	}

    /// <summary>
    /// Receieve pasta based on PPC value.
    /// </summary>
	public void ReceivePasta()
	{
		Pasta += PPC;
	}

    /// <summary>
    /// Receieve and upgrade based on an item
    /// </summary>
    /// <param name="item">Type of item</param>
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

    /// <summary>
    /// Set the app's state
    /// </summary>
    /// <param name="state">State as an integer so it can be called as a UnityEvent</param>
    public void SetState(int state)
    {
        State prevState = this.state;
        this.state = (State)state;
        switch (this.state) {
            case State.Main:
                Resume();
                break;
            case State.Shop:
                Pause();
                break;
            case State.Leaderboard:
                Pause();
                break;
            default:
                break;
        }
        if (StateChanged != null) {
            StateChanged(prevState, this.state);
        }
    }

	private IEnumerator DelayResetPPC(float delay, int resetVal)
	{
		yield return new WaitForSeconds(delay);
		PPC = resetVal;
		if (PowerupOver != null)
			PowerupOver();
	}
}
