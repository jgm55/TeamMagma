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
    public float maxSpeedStart;
    public float maxSpeedBad;
    float timeLava;
    public float timeLavaStart;
    public float timeGoodLava;
    float lavaPercent = .3f;
    float lavaPercentMin = 15f;

	public Sprite mehVolcano;
	public Sprite happyVolcano;
	public Sprite angryVolcano;

	//float goodRatio = 1.2f;
	//float badRatio = 1.2f;
	public float badDevotionLower = 50f;
	public float goodDevotionLower = 50f;

	int DecreaseCount = 5;
	private float counter = 0;
	float MAX_WORSHIP = 60;
	bool draining = false;

    enum TierLevel{LOW, MEDIUM,HIGH};
    TierLevel tierLevel = TierLevel.LOW;

    int eruptCount = 0;

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(1f, .5f);

        changeInWorship = startingWorship;
        maxSpeed = maxSpeedStart;
        timeLava = timeLavaStart;
		FindObjectOfType<Image>().fillAmount = worship/MAX_WORSHIP;

        maxSpeedStart = 1.2f;
        maxSpeedBad = 2.0f;
        timeLavaStart = 8f;
        timeGoodLava = 12f;
	}
	
	// Update is called once per frame
	void Update () {
        fixBugHack();
        //barLevelWorship = worship + startingWorship;

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
            decreasePercentage = .06f;
        }

		goodDevotion = 0;
		badDevotion = 0;

		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
            if (village != null)
            {
                goodDevotion += village.goodDevotion;
                badDevotion += village.badDevotion;
            }
		}

        worship = goodDevotion + badDevotion + changeInWorship;

		if(counter >= DecreaseCount){
            changeInWorship -= Mathf.Max(worship * decreasePercentage, decreaseMin);
			counter = 0;
		}
		if(!instantiated){
			counter += Time.deltaTime;
			// Game Over
            if (worship <= 0f)
            {
				Application.LoadLevel("LoseScreen");
			}
		}

		//Game Win
		if(MAX_WORSHIP <= worship && !instantiated){
            MAX_WORSHIP += MAX_WORSHIP;
            Debug.Log("ERUPTING TO NEXT LEVEL OMG SO DANK");
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(1f, .5f);
            FindObjectOfType<LevelController>().erupt();
            eruptCount++;
            if (FindObjectOfType<LevelController>().levels.Length == eruptCount) {
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
