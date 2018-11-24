using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour {


	public TextMeshProUGUI PastaMain;


	// Use this for initialization
	void Start () {
		GameManager.Instance.PastaChanged += (newVal) => {
			PastaMain.text = newVal.ToString();
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
