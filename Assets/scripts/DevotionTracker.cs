﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DevotionTracker : MonoBehaviour {

	private float devotion = 0;
	public Color color;
	public Font fontType;
	private GUIStyle style;
	Vector2 resolution;
	float resx;
	float resy;
    Rect scoreRect;
    Rect levelRect;
    Vector2 scoreSize = new Vector2(140, 30);
	Vector2 scorePos;

	// Use this for initialization
	void Start () {
		scorePos = new Vector2(1280 - scoreSize.x - 50, 25);
		style = new GUIStyle();
		style.fontSize = 28;
		style.font = fontType;
		style.normal.textColor = color;
		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
        scoreRect = new Rect(scorePos.x * resx, 80 * resy, scoreSize.x * resx, scoreSize.y * resy);
        levelRect = new Rect(20*resx, 80*resy, scoreSize.x * resx, scoreSize.y * resy);
	
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
        GUI.Label(scoreRect, "Score: " + devotion, style);
        GUI.Label(levelRect, "Level: " + (GameObject.FindObjectOfType <LevelController>().levelNumber+1) + " / 5", style);
    }


	void OnTriggerExit2D(Collider2D other){
		Destroy(other.gameObject);
		GameObject.FindGameObjectWithTag("volcano").GetComponent<VolcanoController>().instantiated = false;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().Reset();
	}
}
