using UnityEngine;
using System.Collections;

public class LakeScript : MonoBehaviour {

	public float fillTime = 10;
	public bool isFilling = false;
	public bool isFilled = false;
	private float counter = 0f;
	private float lakeChangeSeconds;
	public Sprite[] lakeFills;
	private int lakeIndex = 0;
	// Use this for initialization
	void Start () {
		lakeChangeSeconds = fillTime / (float)lakeFills.Length ;
	}
	
	// Update is called once per frame
	void Update () {
		if(isFilling){
			if(counter >= fillTime){
				isFilled = true;
				isFilling = false;
			}
			lakeIndex = (int)(counter / lakeChangeSeconds);
			Debug.Log (lakeIndex);
			if(lakeFills.Length > lakeIndex){
				GetComponent<SpriteRenderer>().sprite = lakeFills[lakeIndex];
			}
			counter += Time.deltaTime;

		}
	}

	void OnTriggerExit2D(Collider2D other){
		isFilling = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(!isFilled){
			isFilling = true;
		}
	}
}
