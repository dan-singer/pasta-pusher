using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastaParticle : MonoBehaviour {


    public float yVel = -100;
    public float timeBeforeFadeOut = 1;
    public float fadeOutDuration = 1;
    private float startTime;
    private Vector3 velocity = Vector3.zero;
    private bool isFadingOut = false;
    private CanvasRenderer cRenderer;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        velocity = new Vector3(0, yVel, 0);
        cRenderer = GetComponent<CanvasRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(velocity * Time.deltaTime);
        if (!isFadingOut) {
            if (Time.time > startTime + timeBeforeFadeOut) {
                isFadingOut = true;
                startTime = Time.time;
            }
        }
        else {
            cRenderer.SetAlpha(
                Mathf.Lerp(1, 0, (Time.time - startTime) / fadeOutDuration)
            );
            if (Time.time > startTime + fadeOutDuration) {
                Destroy(gameObject);
                startTime = Time.time;
            }
        }
	}
}
