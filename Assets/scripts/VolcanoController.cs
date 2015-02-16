using UnityEngine;
using System.Collections;

public class VolcanoController : MonoBehaviour {

	public Transform toInstantiate;
	public bool instantiated = false;

	public float startingWorship = 50.0f;
	public float worship = 0;
	public float devotion = 0f;
	public float lavaUsage = 20;
	public float decreaseDevotion = 1.0f;
	int DecreaseCount = 4;
	private float counter = 0;
	int MAX_WORSHIP = 100;

	public Texture devotionTexture;
	Vector2 resolution;
	float resx;
	float resy;
	Vector2 devotionBarPos = new Vector2(20,40);
	Vector2 devotionBarSize;

	Rect devotionBarRect;
	Rect devotionBarCurrentRect;
	// Use this for initialization
	void Start () {
		devotionBarSize = new Vector2(40f,800 - devotionBarPos.y * 2);//max size
		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
		devotionBarRect = new Rect(devotionBarPos.x*resx,devotionBarPos.y*resy,devotionBarSize.x*resx,devotionBarSize.y*resy);
	}
	
	// Update is called once per frame
	void Update () {
		devotion = 0;
		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
			devotion += village.devotion;
		}
		if(counter >= DecreaseCount){
			startingWorship -= decreaseDevotion;
			counter = 0;
		}
		counter += Time.deltaTime;
		worship = devotion + startingWorship;

		if (Input.GetMouseButtonDown (0)){
			//Application.LoadLevel(0);
		}

		// Game Over
		if(worship + startingWorship <= 0f){
			//Application.Quit();
		}

		//Game Win
		if(MAX_WORSHIP < worship + startingWorship){
			//Application.LoadLevel();
		}

		float barHeight = devotionBarRect.height - (devotionBarRect.height * (worship / MAX_WORSHIP));
		devotionBarCurrentRect = new Rect(devotionBarRect.x,barHeight,
		                                  devotionBarRect.width, devotionBarRect.height);
	}

	void OnMouseDown(){
		if (!instantiated) {
			Object lava = Instantiate (toInstantiate);
			CameraControl.myPlay = (Transform)lava;
			instantiated = true;
			startingWorship -= lavaUsage;
		}
	}

//	Vector2 pos = new Vector2(Screen.width / 2 - 50, Screen.height - Screen.height / 15);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void OnGUI(){
		//draw the background:
		//GUI.BeginGroup(new Rect(pos.x, pos.y, pos.x + size.x, pos.y));
		//GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		GUI.BeginGroup (devotionBarRect);
		GUI.DrawTexture(devotionBarCurrentRect,devotionTexture);

		GUI.EndGroup();
		//draw the filled-in part:
		//GUI.BeginGroup(new Rect(0,0, worship, size.y));
		//GUI.Box(new Rect(0,0, worship, size.y), fullTex);
		//GUI.EndGroup();
		//GUI.EndGroup();
	}
}
