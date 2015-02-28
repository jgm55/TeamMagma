using UnityEngine;
using System.Collections;

public class tutorialController : MonoBehaviour {


	//public float minSwipeDist = .1f, maxSwipeTime = 2f; 
	//bool couldBeSwipe;

	private Vector3 lastTouchPos = new Vector3(0,0,0);


	public string nextLevel;
	public string previousLevel = "";

	// Use this for initialization
	void Start () {
		Debug.Log("Start");

		//StartCoroutine(checkHorizontalSwipes());
		Debug.Log("Start2");

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonUp(0) && !lastTouchPos.Equals(new Vector3(0,0,0))){
			if(lastTouchPos.x > Input.mousePosition.x){
				RightSwipe();
			} else if(lastTouchPos.x < Input.mousePosition.x){
				LeftSwipe();
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			//if(Input.GetMouseButtonDown(1)){
			//}
			lastTouchPos = Input.mousePosition;
		}
	}

	void RightSwipe(){
		Debug.Log("Right Swipe");
		Application.LoadLevel(nextLevel);
	}

	void LeftSwipe(){
		Debug.Log("left Swipe");

		if(previousLevel != ""){
			Application.LoadLevel(previousLevel);	
		}
	}
/*
	IEnumerator checkHorizontalSwipes () //Coroutine, wich gets Started in "Start()" and runs over the whole game to check for swipes
	{
		Debug.Log("Starting");
		Vector2 startPos = new Vector2(0,0);
		float swipeStartTime = 0.0f;
		while (true) { //Loop. Otherwise we wouldnt check continoulsy ;-)
			foreach (Touch touch in Input.touches) { //For every touch in the Input.touches - array...
				
				switch (touch.phase) {
				case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
					couldBeSwipe = true;
					
					startPos = touch.position;  //Position where the touch started
					swipeStartTime = Time.time; //The time it started
					break;
					
				case TouchPhase.Stationary: //Is the touch stationary? --> No swipe then!
					couldBeSwipe = false;
					break;
				}
				
				float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
				float swipeDist = Mathf.Abs (touch.position.x - startPos.x); //Swipedistance
				
				
				if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist) {
					// It's a swiiiiiiiiiiiipe!
					couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.
					
					if (Mathf.Sign (touch.position.x - startPos.x) == 1f) { //Swipe-direction, either 1 or -1.
						
						RightSwipe();

					} else {
						
						//Left-swipe
						LeftSwipe();
					}
				} 
			}
			yield return null;
		}
	}*/
}
