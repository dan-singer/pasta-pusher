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

	// Use this for initialization
	void Start () {
		// Keep pasta in sync with the model
		GameManager.Instance.PastaChanged += (newVal) => {
			PastaMain.text = newVal.ToString();
			PastaShopText.text = "In Pot: " + newVal.ToString();
		};

		// Logic for clicking inventory items
		foreach (Item item in GameManager.Instance.ShopItems){
			GameObject uiItem = Instantiate(UIItemPrefab, ItemsParent);
			uiItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = item.Name;
			uiItem.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = item.Description;
			uiItem.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = item.Cost.ToString();
			
			Button b = uiItem.GetComponent<Button>();


			// We need to set colors on pointer enter because click will not set them before click
			var pointerEnterEntry = new EventTrigger.Entry();
			pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
			pointerEnterEntry.callback.AddListener((eventData) => {
				ColorBlock colors = b.colors;
				if (GameManager.Instance.Pasta >= item.Cost){
					colors.pressedColor = Color.green;
					b.colors = colors;
				} else{
					colors.pressedColor = Color.red;
					b.colors = colors;
				}
			});
			
			b.onClick.AddListener(() => {
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
