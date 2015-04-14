using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.scripts;

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
	private float magmaFillScaler = .04f;

    float maxSpeed;
    float maxSpeedStart = 1.2f;
    float maxSpeedBad = 2f;
    float timeLava;
    float timeLavaStart = 8f;
    float timeGoodLava = 15f;

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
	float MAX_WORSHIP = 200;
	bool draining = false;

    enum TierLevel{LOW, MEDIUM,HIGH};
    TierLevel tierLevel = TierLevel.LOW;

	// Use this for initialization
	void Start () {
        barLevelWorship = worship + startingWorship;
        maxSpeed = maxSpeedStart;
        timeLava = timeLavaStart;
		FindObjectOfType<Image>().fillAmount = barLevelWorship/MAX_WORSHIP;
	}
	
	// Update is called once per frame
	void Update () {
        barLevelWorship = worship + startingWorship;

        float third = MAX_WORSHIP / 3;
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
            maxSpeed = maxSpeedStart;
            timeLava = timeGoodLava;
            Properties.lastPlayedStyle = Properties.PlayStyle.GOOD;
		} else if(badDevotion - goodDevotion > badDevotionLower){
			spriteRender.sprite = angryVolcano;
            maxSpeed = maxSpeedBad;
            timeLava = timeLavaStart;
            Properties.lastPlayedStyle = Properties.PlayStyle.BAD;
		} else {
			spriteRender.sprite = mehVolcano;
            maxSpeed = maxSpeedStart;
            timeLava = timeLavaStart;
            Properties.lastPlayedStyle = Properties.PlayStyle.NUETRAL;
		}
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
            lava.GetComponent<AccelControl>().maxVelocity = maxSpeed;
            lava.GetComponent<AccelControl>().timeLava = timeLava;
			draining = true;
		}
	}

	void OnGUI(){
		Image Image  = FindObjectOfType<Image> ();
		Debug.Log (barLevelWorship);
		
		Image.fillAmount = Mathf.MoveTowards (Image.fillAmount, barLevelWorship/MAX_WORSHIP, Time.deltaTime * magmaFillScaler);
	}
}
