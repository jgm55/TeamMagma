using UnityEngine;
using System.Collections;

public class VolcanoController : MonoBehaviour {

	public GameObject toInstantiate;
	public bool instantiated = false;

	float startingWorship = 50.0f;
	float worship = 0;
    float barLevelWorship = 0f;
	public float goodDevotion = 0f;
	public float badDevotion = 0f;
	float lavaUsage = 15f;
	float decreasePercentage = .01f;

	public Sprite mehVolcano;
	public Sprite happyVolcano;
	public Sprite angryVolcano;

	//float goodRatio = 1.2f;
	//float badRatio = 1.2f;
	float badDevotionLower = 50f;
	float goodDevotionLower = 50f;

    float lavaPercent = .1f;
	int DecreaseCount = 5;
	private float counter = 0;
	int MAX_WORSHIP = 200;
//	float drainTime = 1f;
//	float drainCounter = 0;
	bool draining = false;

	public Texture devotionTexture;
	Vector2 resolution;
	float resx;
	float resy;
	Vector2 devotionBarPos = new Vector2(20,40);
	Vector2 devotionBarSize;

    enum TierLevel{LOW, MEDIUM,HIGH};
    TierLevel tierLevel = TierLevel.LOW;

	Rect devotionBarRect;
	Rect devotionBarCurrentRect;
	// Use this for initialization
	void Start () {
		devotionBarSize = new Vector2(40f,800 - devotionBarPos.y * 2);//max size
		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
		devotionBarRect = new Rect(devotionBarPos.x*resx,devotionBarPos.y*resy,devotionBarSize.x*resx,devotionBarSize.y*resy);

        barLevelWorship = worship + startingWorship;
	}
	
	// Update is called once per frame
	void Update () {
        barLevelWorship = worship + startingWorship;
		if(!draining){
            //drainCounter = 0;
		}

        int third = MAX_WORSHIP / 3;
        if(barLevelWorship < third){
            tierLevel = TierLevel.LOW;
            lavaPercent = .1f;
            decreasePercentage = .01f;
        }
        else if (barLevelWorship < 2 * third)
        {
            tierLevel = TierLevel.MEDIUM;
            lavaPercent = .2f;
            decreasePercentage = .02f;

        } else {
            tierLevel = TierLevel.HIGH;
            lavaPercent = .3f;
            decreasePercentage = .03f;
        }

		goodDevotion = 0;
		badDevotion = 0;

		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
			goodDevotion += village.goodDevotion;
			badDevotion += village.badDevotion;
		}
		if(counter >= DecreaseCount){
			startingWorship -= barLevelWorship*decreasePercentage;
			counter = 0;
		}
		if(!instantiated){
			counter += Time.deltaTime;
			// Game Over
			if(barLevelWorship <= 0f){
				//Application.Quit();
				Application.LoadLevel("LoseScreen");
			}
		}
		worship = goodDevotion + badDevotion + startingWorship;

		//Game Win
		if(MAX_WORSHIP < barLevelWorship){
			Application.LoadLevel("WinScreen");
		}

		SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
		if(goodDevotion - badDevotion > goodDevotionLower){
			spriteRender.sprite = happyVolcano;
		} else if(badDevotion - goodDevotion > badDevotionLower){
			spriteRender.sprite = angryVolcano;
		} else {
			spriteRender.sprite = mehVolcano;
		}

		//TODO look at this later
		float barHeight = (devotionBarRect.height - (devotionBarRect.height * ((worship) / MAX_WORSHIP))) *resy;
		devotionBarCurrentRect = new Rect(devotionBarRect.x,barHeight,
		                                  devotionBarRect.width, devotionBarRect.height);
	}

	void OnMouseDown(){
		if (!instantiated) {
			Debug.Log("instantiating lava");
			//Transform t = ((Transform)(Instantiate (toInstantiate)));
			GameObject lava = Instantiate (toInstantiate) as GameObject;
			CameraControl.myPlay = lava;
			//TODO Add this to heirarchy
			GameObject level = GameObject.FindGameObjectWithTag ("level");
			lava.transform.parent = level.transform;
			instantiated = true;
			startingWorship -= lavaPercent * barLevelWorship;
			draining = true;
		}
	}

	void OnGUI(){
        //TODO Josh, hook Devotion Bar to here using var worship
		GUI.BeginGroup (devotionBarRect);
		GUI.DrawTexture(devotionBarCurrentRect,devotionTexture);
		GUI.EndGroup();
	}
}
