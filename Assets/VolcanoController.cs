using UnityEngine;
using System.Collections;

public class VolcanoController : MonoBehaviour {

	public Transform toInstantiate;
	public bool instantiated = false;

	private float worship = 50.0f;
	public float lavaUsage = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)){
			//Application.LoadLevel(0);
		}
		if(worship <= 0f){
			Application.Quit();
		}
	}

	void OnMouseDown(){
		if (!instantiated) {
			Object lava = Instantiate (toInstantiate);
			CameraControl.myPlay = (Transform)lava;
			instantiated = true;
			worship -= lavaUsage;
		}
	}

	public Vector2 pos = new Vector2(Screen.width / 2 - 50, Screen.height - Screen.height / 15);
	public Vector2 size = new Vector2(100,20);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void OnGUI(){
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, pos.x + size.x, pos.y + size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, worship, size.y));
		GUI.Box(new Rect(0,0, worship, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
	}
}
