using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour {


	public TextMeshProUGUI Pasta;


	// Use this for initialization
	void Start () {
		GameManager.Instance.PastaChanged += (newVal) => {
			Pasta.text = newVal.ToString();
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
