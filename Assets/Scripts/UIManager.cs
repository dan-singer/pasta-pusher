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

    private int prevPasta = 0;
    private Dictionary<Item, UIItem> ItemMapping;

	// Use this for initialization
	void Awake () {
		// Keep pasta in sync with the model
		GameManager.Instance.PastaChanged += (newVal) => {
			PastaMain.text = newVal.ToString();
			PastaShopText.text = "In Pot: " + newVal.ToString();
            int delta = newVal - prevPasta;
            for (int i = 0; i < delta; ++i) {
                PastaFeedbackSpawner.Spawn();
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

        // Logic for clicking inventory items
        ItemMapping = new Dictionary<Item, UIItem>();
		foreach (Item item in GameManager.Instance.ShopItems){
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
				if (GameManager.Instance.Pasta >= item.Cost){
					colors.pressedColor = Color.green;
					itemButtonComponent.colors = colors;
				} else{
					colors.pressedColor = Color.red;
					itemButtonComponent.colors = colors;
				}
			});
			
			itemButtonComponent.onClick.AddListener(() => {
				if (GameManager.Instance.Pasta >= item.Cost){
					GameManager.Instance.ReceiveUpgrade(item);
				}
			});

			uiItem.GetComponent<EventTrigger>().triggers.Add(pointerEnterEntry);

		}
	}

	public void ToggleUIPanel(GameObject panel)
	{
		panel.GetComponent<Animator>().SetTrigger("Toggle");
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
		
	}
}
