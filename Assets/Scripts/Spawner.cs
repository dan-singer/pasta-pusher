using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject ToSpawn;

	// Use this for initialization
	void Start () {
	}

    public void Spawn()
    {
        RectTransform rt = GetComponent<RectTransform>();
        print(rt.sizeDelta);
        Vector2 spawnMin = -rt.sizeDelta / 2;
        Vector2 spawnMax = rt.sizeDelta / 2;
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x), 
            Random.Range(spawnMin.y, spawnMax.y)
            );
        GameObject spawnedObj = Instantiate(ToSpawn);
        spawnedObj.transform.SetParent(rt, false);
        spawnedObj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
