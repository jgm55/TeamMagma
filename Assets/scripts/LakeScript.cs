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
    public AudioSource audioSource;

	float startVelocity;

	float startDrag = 1;
	float lakeDrag = 2;

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
//			Debug.Log (lakeIndex);
			if(lakeFills.Length > lakeIndex){
				GetComponent<SpriteRenderer>().sprite = lakeFills[lakeIndex];
			}
			counter += Time.deltaTime;
		}
	}

    public void OnTriggerExit2D(Collider2D other)
    {
		isFilling = false;
		other.gameObject.GetComponent<Rigidbody2D>().drag = startDrag;
		AccelControl script = other.GetComponent<AccelControl>();
		script.maxVelocity = startVelocity;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(.01f);
        audioSource.Stop();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(!isFilled){
            audioSource.Play();
			isFilling = true;
			other.gameObject.GetComponent<Rigidbody2D>().drag = lakeDrag;
			AccelControl script = other.GetComponent<AccelControl>();
			startVelocity = script.maxVelocity;
			script.maxVelocity = startVelocity / 2;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(.01f);
		}
	}
}
