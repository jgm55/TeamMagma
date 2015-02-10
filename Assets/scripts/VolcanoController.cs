using UnityEngine;
using System.Collections;

public class VolcanoController : MonoBehaviour {

	public Transform toInstantiate;
	public bool instantiated = false;

	public float startingWorship = 50.0f;
	public float worship = 0;
	public float lavaUsage = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float devotion = 0;
		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
			devotion += village.devotion;
		}
		worship = devotion + startingWorship;

		if (Input.GetMouseButtonDown (0)){
			//Application.LoadLevel(0);
		}
		if(worship + startingWorship <= 0f){
			//Application.Quit();
		}
	}

	void OnMouseDown(){
		if (!instantiated) {
			Object lava = Instantiate (toInstantiate);
			CameraControl.myPlay = (Transform)lava;
			instantiated = true;
			startingWorship -= lavaUsage;
		}
	}

	Vector2 pos = new Vector2(Screen.width / 2 - 50, Screen.height - Screen.height / 15);
	Vector2 size = new Vector2(100,20);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void OnGUI(){
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, pos.x + size.x, pos.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, worship, size.y));
		GUI.Box(new Rect(0,0, worship, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
	}
}
