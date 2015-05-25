using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.scripts;

public class VolcanoController : MonoBehaviour {

	public GameObject toInstantiate;
	public bool instantiated = false;

    public AudioClip eruptSound;
    float startingWorship = 50.0f;
    float changeInWorship;
	float worship = 0;
	public float goodDevotion = 0f;
	public float badDevotion = 0f;
	float lavaUsage = 15f;
	float decreasePercentage = .02f;
    float decreaseMin = 5f;

	private float magmaFillScaler = .04f;

    float maxSpeed;
    float maxSpeedStart = 1.2f;
    float maxSpeedBad = 2f;
    float timeLava;
    float timeLavaStart = 8f;
    float timeGoodLava = 15f;
    float lavaPercent = .3f;
    float lavaPercentMin = 15f;

	public Sprite mehVolcano;
	public Sprite happyVolcano;
	public Sprite angryVolcano;

	//float goodRatio = 1.2f;
	//float badRatio = 1.2f;
	float badDevotionLower = 50f;
	float goodDevotionLower = 50f;

	int DecreaseCount = 5;
	private float counter = 0;
	float MAX_WORSHIP = 75;
	bool draining = false;

    enum TierLevel{LOW, MEDIUM,HIGH};
    TierLevel tierLevel = TierLevel.LOW;

    int eruptCount = 0;

	// Use this for initialization
	void Start () {
        changeInWorship = startingWorship;
        maxSpeed = maxSpeedStart;
        timeLava = timeLavaStart;
		FindObjectOfType<Image>().fillAmount = worship/MAX_WORSHIP;
	}
	
	// Update is called once per frame
	void Update () {
        fixBugHack();
        //barLevelWorship = worship + startingWorship;
        worship = goodDevotion + badDevotion + changeInWorship;

        float third = MAX_WORSHIP / 3;
        if(worship < third){
            tierLevel = TierLevel.LOW;
            lavaPercent = .3f;
            decreasePercentage = .02f;
        }
        else if (worship < 2 * third)
        {
            tierLevel = TierLevel.MEDIUM;
            lavaPercent = .3f;
            decreasePercentage = .04f;

        } else {
            tierLevel = TierLevel.HIGH;
            lavaPercent = .3f;
            decreasePercentage = .08f;
        }

		goodDevotion = 0;
		badDevotion = 0;

		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
			goodDevotion += village.goodDevotion;
			badDevotion += village.badDevotion;
		}
		if(counter >= DecreaseCount){
            changeInWorship -= Mathf.Max(worship * decreasePercentage, decreaseMin);
			counter = 0;
		}
		if(!instantiated){
			counter += Time.deltaTime;
			// Game Over
            if (worship <= 0f)
            {
				//Application.Quit();
				Application.LoadLevel("LoseScreen");
			}
		}

		//Game Win
		if(MAX_WORSHIP <= worship){
            MAX_WORSHIP += MAX_WORSHIP;
            Debug.Log("ERUPTING TO NEXT LEVEL OMG SO DANK");
            FindObjectOfType<LevelController>().erupt();
            eruptCount++;
            if (FindObjectOfType<LevelController>().levels.Length == eruptCount) {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(.8f, .1f);
                Debug.Log("End Game: " + Properties.lastPlayedStyle);
                if(Properties.lastPlayedStyle == Properties.PlayStyle.BAD){
                    Application.LoadLevel("WinScreenBad");
                }
                else if (Properties.lastPlayedStyle == Properties.PlayStyle.GOOD)
                {
                    Application.LoadLevel("WinScreenGood");
                }
                else
                {
                    Application.LoadLevel("WinScreen");
                }
            }
            //worship = startingWorship;
            if (instantiated)
            {
                //Destroy(FindObjectOfType<AccelControl>().gameObject);
            }
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
            instantiated = true;
            AudioSource.PlayClipAtPoint(eruptSound, this.transform.position);
			//Transform t = ((Transform)(Instantiate (toInstantiate)));
			GameObject lava = Instantiate (toInstantiate) as GameObject;
			CameraControl.myPlay = lava;
			//TODO Add this to heirarchy
			GameObject level = GameObject.FindGameObjectWithTag ("level");
			lava.transform.parent = level.transform;
            changeInWorship -= getLavaDecreaseAmount();
            lava.GetComponent<AccelControl>().maxVelocity = maxSpeed;
            lava.GetComponent<AccelControl>().timeLava = timeLava;
			draining = true;
		}
	}

    private float getLavaDecreaseAmount()
    {
        return Mathf.Max(lavaPercent * worship, lavaPercentMin);
    }

	void OnGUI(){
		Image[] images  = FindObjectsOfType<Image> ();
        foreach (Image image in images)
        {
            if (image.type == Image.Type.Filled)
            {
                float target = worship / MAX_WORSHIP;
                if (image.tag == "barOverlay")
                {
                    image.fillAmount = Mathf.MoveTowards(image.fillAmount, target, Time.deltaTime * magmaFillScaler);
                }
                else
                {
                    image.fillAmount = Mathf.MoveTowards(image.fillAmount, target - getLavaDecreaseAmount() / MAX_WORSHIP, Time.deltaTime * magmaFillScaler);
                }
            }
        }
	}

    private void fixBugHack(){
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos2 = new Vector3(pos.x, pos.y,-20);
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, pos2);
            Debug.Log("hits: " + hits);
            foreach(RaycastHit2D hit in hits){
                Debug.Log("hit" + hit.collider.gameObject);
                VolcanoController volcano = hit.collider.gameObject.GetComponent<VolcanoController>();
                if (volcano != null)
                {
                    volcano.OnMouseDown();
                }
            }
        }
    }
}
