using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DevotionTracker : MonoBehaviour {

	private float devotion = 0;
	Vector2 resolution;
	float resx;
	float resy;
	Rect scoreRect;
	Vector2 scoreSize = new Vector2(140,30);
	Vector2 scorePos;

	// Use this for initialization
	void Start () {
		scorePos = new Vector2(1280 - scoreSize.x, 20);

		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
		scoreRect = new Rect(scorePos.x*resx,scorePos.y*resy,scoreSize.x*resx,scoreSize.y*resy);
	
	}
	
	// Update is called once per frame
	void Update () {
		VolcanoController volcano = GameObject.FindGameObjectWithTag ("volcano").GetComponent<VolcanoController> ();
		devotion = volcano.goodDevotion + volcano.badDevotion;
	}



	void OnGUI(){
		//GUI.BeginGroup(scoreRect);
		//Text text = GetComponent<Text>();
		//text.text = "Score: " + devotion.ToString();
		GUIStyle style = new GUIStyle();
		style.fontSize = 20;
		GUI.Label(scoreRect, "Score: " + devotion.ToString(), style);
	}


	void OnTriggerExit2D(Collider2D other){
		Destroy(other.gameObject);
		GameObject.FindGameObjectWithTag("volcano").GetComponent<VolcanoController>().instantiated = false;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().Reset();
	}

}
