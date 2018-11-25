using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class UIManager : MonoBehaviour {


	public TextMeshProUGUI PastaMain, PastaShopText;
	public RectTransform ItemsParent;
	public GameObject UIItemPrefab;

	// Use this for initialization
	void Start () {
		GameManager.Instance.PastaChanged += (newVal) => {
			PastaMain.text = newVal.ToString();
			PastaShopText.text = "In Pot: " + newVal.ToString();
		};
		foreach (Item item in GameManager.Instance.ShopItems){
			GameObject uiItem = Instantiate(UIItemPrefab, ItemsParent);
			uiItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = item.Name;
			uiItem.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = item.Description;
			uiItem.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = item.Cost.ToString();

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => {
				if (GameManager.Instance.Pasta >= item.Cost){
					GameManager.Instance.ReceiveUpgrade(item);
				}
			});

			uiItem.GetComponent<EventTrigger>().triggers.Add(entry);
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
