using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public enum CameraState{NUETRAL, FOLLOWING};

	Vector3 myPos = new Vector3(0,-2,-15);
	public static GameObject myPlay;
//	private bool isZoomed = false;
	float SPIN_TIME = .5f;
	//int zoom = 5; int normal = 10; float smooth = 5; 
	//Vector3 zoom = new Vector3(0,0,-10), normal = new Vector3(0,0,100);
	private Vector3 lastTouchPos = new Vector3(0,0,0);
	// Use this for initialization
//	float doubleClickDelay = .5f;
//	float lastDown = -2.0f;
//	private float totalMoved = 0f;
	private float spinCount = 0;
	private float angle=0f;
    Vector3 originalCameraPosition; 
    float shakeAmt = .05f;
    bool shaking = false;
    float shakeCount = 0f;
    float shakeTime = 1f;
    float zoomSpeed = 10f;
	public CameraState state = CameraState.NUETRAL;

	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// zoom in and follow the lava head
		if(GameObject.FindGameObjectWithTag("volcano").GetComponent<VolcanoController>().instantiated){
            GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize - zoomSpeed*Time.deltaTime,5);
			transform.position = myPlay.transform.position + myPos;
			state = CameraState.FOLLOWING;
		} else {
			Reset();
		}


		if(myPlay == null && Input.GetMouseButtonUp(0) && !lastTouchPos.Equals(new Vector3(0,0,0))){
			angle = Vector3.Angle(lastTouchPos, Input.mousePosition);
			if(lastTouchPos.x > Input.mousePosition.x){
				angle *= -1;
			}
		}

		if(angle != 0f){
			if(spinCount >= SPIN_TIME){
				spinCount = 0f;
				angle = 0f;
			} else {
				GameObject level = GameObject.FindGameObjectWithTag("level");
				level.transform.RotateAround(level.transform.position,new Vector3(0,0,1), Time.deltaTime * angle * 1/SPIN_TIME);
				spinCount += Time.deltaTime;
			}
		}
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPos = Input.mousePosition;
        }
		if(shaking && shakeCount < shakeTime){
            float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;
            Vector3 pp = this.transform.position;
            if (Random.Range(0, 2) == 0)
            {
                pp.y += quakeAmt;
            }
            else
            {
                pp.x += quakeAmt;
            }
            this.transform.position = pp;
            shakeCount += Time.deltaTime;
        }
        else if(shakeCount > shakeTime)
        {
            shaking = false;
            shakeCount = 0;
        }
	}

	public void Reset(){
		//this.transform.position = GameObject.FindGameObjectWithTag("volcano").transform.position + myPos;
		myPlay = null;
		state = CameraState.NUETRAL;
		GetComponent<Camera>().orthographicSize = 15;
		transform.position = new Vector3(-.96f,-.12f,-15);
	}

    public void startShakingCamera(float maxTime=.2f, float shakeAmount = .05f)
    {
        shaking = true;
        shakeTime = maxTime;
    }

    public void stopShakingCamera()
    {
        shaking = false;
        this.transform.position = originalCameraPosition;
    }
}
