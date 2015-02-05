using UnityEngine;
using System.Collections;

public class LakeScript : MonoBehaviour {

	public float fillTime = 10;
	private bool isFilling = false;
	private bool isFilled = false;
	private float counter = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isFilling){
			if(counter >= fillTime){
				isFilled = true;
				isFilling = false;
			}
			counter += Time.deltaTime;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		isFilling = false;
	}

	void onTriggerEnter2D(Collider2D other){
		if(!isFilled){
			isFilling = true;
		}
	}
}
