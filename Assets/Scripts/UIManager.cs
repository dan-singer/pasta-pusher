using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This is the View of the application
/// </summary>
public class UIManager : MonoBehaviour {


	public TextMeshProUGUI PastaMain, PastaShopText;
	public RectTransform ItemsParent;
	public GameObject UIItemPrefab;
    public Spawner PastaFeedbackSpawner;
    public GameObject NotificationMarker;
    public GameObject ShopPanel, LeaderboardPanel; 

    private int prevPasta = 0;
    private Dictionary<Item, UIItem> ItemMapping;

    private Vector3 fp; //First touch position
    private Vector3 lp; //Last touch position
    private float dragDistance = Screen.height * .15f; 

	// Use this for initialization
	void Awake () {
		// Keep pasta in sync with the model
		GameManager.Instance.PastaChanged += (newVal) => {
			PastaMain.text = newVal.ToString();
			PastaShopText.text = "In Pot: " + newVal.ToString();
            int delta = newVal - prevPasta;
            if (delta > 0)
            {
                PastaParticle particle = PastaFeedbackSpawner.Spawn().GetComponent<PastaParticle>();
                particle.quantityText.text = "+" + delta;
            }
            prevPasta = newVal;
            foreach (Item key in ItemMapping.Keys)
            {
                bool interactable = newVal >= key.Cost;
                ItemMapping[key].MainButton.interactable = interactable;
            }
		};

        GameManager.Instance.ItemBecameAvailable += (item) =>
        {
            SetShopNotification(true);
        };

        BuildShop();

        GameManager.Instance.StateChanged += (prevState, state) =>
        {
            switch (state) {
                case GameManager.State.Main:
                    if (prevState == GameManager.State.Shop)
                        HideUIPanel(ShopPanel);
                    else if (prevState == GameManager.State.Leaderboard)
                        HideUIPanel(LeaderboardPanel);
                    break;
                case GameManager.State.Shop:
                    SetShopNotification(false);
                    ShowUIPanel(ShopPanel);
                    break;
                case GameManager.State.Leaderboard:
                    ShowUIPanel(LeaderboardPanel);
                    break;
                default:
                    break;
            }
        };

	}

    private void BuildShop()
    {
        // Logic for clicking inventory items
        ItemMapping = new Dictionary<Item, UIItem>();
        foreach (Item item in GameManager.Instance.ShopItems) {
            GameObject itemGO = Instantiate(UIItemPrefab, ItemsParent);
            UIItem uiItem = itemGO.GetComponent<UIItem>();
            uiItem.Title.text = item.Name;
            uiItem.Description.text = item.Description;
            uiItem.Cost.text = item.Cost.ToString();

            ItemMapping[item] = uiItem;

            Button itemButtonComponent = uiItem.GetComponent<Button>();

            // We need to set colors on pointer enter because click will not set them before click
            var pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => {
                ColorBlock colors = itemButtonComponent.colors;
                if (GameManager.Instance.Pasta >= item.Cost) {
                    colors.pressedColor = Color.green;
                    itemButtonComponent.colors = colors;
                }
                else {
                    colors.pressedColor = Color.red;
                    itemButtonComponent.colors = colors;
                }
            });

            itemButtonComponent.onClick.AddListener(() => {
                if (GameManager.Instance.Pasta >= item.Cost) {
                    GameManager.Instance.ReceiveUpgrade(item);
                }
            });

            uiItem.GetComponent<EventTrigger>().triggers.Add(pointerEnterEntry);

        }
    }

	public void ShowUIPanel(GameObject panel)
	{
		panel.GetComponent<Animator>().SetTrigger("Enter");
	}
    public void HideUIPanel(GameObject panel)
    {
        panel.GetComponent<Animator>().SetTrigger("Exit");
    }

    public void SetShopNotification(bool visible)
    {
        if (NotificationMarker)
        {
            NotificationMarker.GetComponent<Image>().enabled = visible;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Swiping code based on https://forum.unity.com/threads/simple-swipe-and-tap-mobile-input.376160/ 
        // Check for swipe left or right
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) {
                lp = touch.position;
                // Check if it's a drag
                float absDx = Mathf.Abs(lp.x - fp.x);
                float absDy = Mathf.Abs(lp.y - fp.y);
                if (absDx > dragDistance || absDy > dragDistance) {
                    if (absDx > absDy) {
                        // Swipe to the right
                        if (lp.x > fp.x) {
                            if (GameManager.Instance.state == GameManager.State.Main) {
                                GameManager.Instance.SetState((int)GameManager.State.Shop);
                            }
                        }
                        // Swipe to the left
                        else {
                            if (GameManager.Instance.state == GameManager.State.Main) {
                                GameManager.Instance.SetState((int)GameManager.State.Leaderboard);
                            }
                        }
                    }
                }
            }
        }
	}
}
