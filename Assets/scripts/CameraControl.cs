using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public enum CameraState{NUETRAL, FOLLOWING};

	Vector3 myPos = new Vector3(0,-2,-15);
	public static Transform myPlay;
	private bool isZoomed = false;
	//int zoom = 5; int normal = 10; float smooth = 5; 
	//Vector3 zoom = new Vector3(0,0,-10), normal = new Vector3(0,0,100);
	private Vector3 lastTouchPos = new Vector3(0,0,0);
	// Use this for initialization
	float doubleClickDelay = .5f;
	float lastDown = -2.0f;

	public CameraState state = CameraState.NUETRAL;

	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		state = CameraState.NUETRAL;
		if(myPlay != null){
			camera.orthographicSize = 5;
			transform.position = myPlay.position + myPos;
			state = CameraState.FOLLOWING;
		}
		if(state == CameraState.NUETRAL){
			camera.orthographicSize = 15;
			transform.position.Set(-0.7f,-2.44f, -15f);
		}
		/*
		if(Input.GetMouseButtonUp(0) && !lastTouchPos.Equals(new Vector3(0,0,0))){
			float z = myPos.z;
			float x = (transform.position.x + lastTouchPos.x - Input.mousePosition.x) / Mathf.Abs(z) / camera.orthographicSize;
			float y = (transform.position.y + lastTouchPos.y - Input.mousePosition.y) / Mathf.Abs(z) / camera.orthographicSize;
			//transform.position = new Vector3(x,y,z);
		}
		if (Input.GetMouseButtonDown (0)) {
			//if(Input.GetMouseButtonDown(1)){
			//}
			lastTouchPos = Input.mousePosition;
			//double tap to zoom
			if(state != CameraState.FOLLOWING){
				if(Time.time - lastDown < doubleClickDelay){
					lastDown = -2.0f;
					isZoomed = !isZoomed;
					if (isZoomed == true) {
						//camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,zoom,Time.deltaTime*smooth);
						camera.orthographicSize = 15;
						transform.position = new Vector3(-0.7,-2.44,-15);
					} else {
						//camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,normal,Time.deltaTime*smooth);
						camera.orthographicSize = 5;
					}
				} else {
					lastDown = Time.time;

				}
			}
		}*/
	}

	public void Reset(){
		this.transform.position = GameObject.FindGameObjectWithTag("volcano").transform.position + myPos;
		myPlay = null;
	}
}
