using UnityEngine;
using System.Collections;

public class VillageController : MonoBehaviour {
	
	public enum VillageState{BURNING, NUETRAL, WORSHIPPING};
	public GameObject lake;
	public GameObject[] houses;
	public GameObject house;
	public GameObject positiveWorship;
	public GameObject negativeWorship;
	public GameObject alertBubble;
	public GameObject doublePositiveWorship;
	public GameObject doubleNegativeWorship;
    private GameObject villageGround;
    public AudioSource audioSource;

    public GameObject[] goodCircles;
    public GameObject[] badCircles;
    private int circleIndex;

    float maxScaleGround = 4f;

	public int goodDevotion = 0;
	public int badDevotion = 0;
	int devotionRateBad = 1;
	int devotionRateGood = 1;
	int forgetDevotion = 20;
	int maxHouses = 6;
	float radiusNear = 6.0f;
	int MAX_ENCOUNTER = 2;

	private float forgetCounter = 0;
	private float devotionCounter = 0;
	private float houseCounter = 0;
	private int timesEncounter = 0;
	private bool wasFilling = false;
	
	public VillageState state = VillageState.NUETRAL;

	int devotionIncrement = 5;
	int makeHouseSeconds = 12;


	public float textCounter = 0f;
	float textSeconds = .5f;
	float pointsGained = 0f;
	bool updatedDevotion = false;

	Vector2 resolution;
	float resx;
	float resy;
	Rect pointsRect;
	Vector2 pointsSize = new Vector2(30,30);
	Vector2 pointsPos;

	// Use this for initialization
	void Start () {
        circleIndex = Random.Range(0, badCircles.Length);
        /*villageGround = new GameObject();
        villageGround.transform.SetParent(this.transform);
        villageGround.transform.position = this.transform.position;
        villageGround.transform.rotation = this.transform.rotation;
        SpriteRenderer renderer = villageGround.AddComponent<SpriteRenderer>();
        renderer.sortingLayerID = -1;*/

		pointsPos = new Vector2(this.transform.position.x + 2.1f, this.transform.position.y + 2.1f);
		resolution = new Vector2(Screen.width, Screen.height);
		resx = resolution.x/1280.0f; // 1280 is the x value of the working resolution
		resy = resolution.y/800.0f; // 800 is the y value of the working resolution
		pointsRect = new Rect(pointsPos.x*resx,pointsPos.y*resy,pointsSize.x*resx,pointsSize.y*resy);

	}
	
	// Update is called once per frame
	void Update () {
		if(houses.Length == 0){
			state = VillageState.NUETRAL;
		}

		if(state != VillageState.NUETRAL){
			devotionCounter += Time.deltaTime;
			forgetCounter += Time.deltaTime;
		} //else {
		positiveWorship.GetComponent<Renderer>().enabled = false;
		negativeWorship.GetComponent<Renderer>().enabled = false;
		doublePositiveWorship.GetComponent<Renderer>().enabled = false;
		doubleNegativeWorship.GetComponent<Renderer>().enabled = false;
		alertBubble.GetComponent<Renderer>().enabled = false;

        if(state == VillageState.BURNING){
            audioSource.Play();
            Destroy(villageGround);
            villageGround = Instantiate(badCircles[circleIndex], transform.position, transform.rotation) as GameObject;
            villageGround.transform.SetParent(this.transform);
            if (goodDevotion + badDevotion != 0)
            {
                float scaleAmount = (badDevotion - goodDevotion) / (goodDevotion + badDevotion) * maxScaleGround;
                scaleAmount = (1 - ((MAX_ENCOUNTER - timesEncounter) * forgetDevotion + forgetCounter) /
                    (forgetDevotion * MAX_ENCOUNTER)) * maxScaleGround;
                if (scaleAmount > 0)
                {
                    villageGround.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
                }
            }
			if(timesEncounter > 1){
				doubleNegativeWorship.GetComponent<Renderer>().enabled = true;
				Color c = doubleNegativeWorship.GetComponent<Renderer>().material.color;
				c.a = devotionCounter / devotionIncrement;
				doubleNegativeWorship.GetComponent<Renderer>().material.color = c;
			} else if(timesEncounter == 1){
				negativeWorship.GetComponent<Renderer>().enabled = true;
				Color c = negativeWorship.GetComponent<Renderer>().material.color;
				c.a = devotionCounter / devotionIncrement;
				negativeWorship.GetComponent<Renderer>().material.color = c;
			}
		} else if(state == VillageState.WORSHIPPING){
            audioSource.Stop();
            Destroy(villageGround);
            villageGround = Instantiate(goodCircles[circleIndex], transform.position, transform.rotation) as GameObject;
            villageGround.transform.SetParent(this.transform);
            if (goodDevotion + badDevotion != 0)
            {
                float scaleAmount = (goodDevotion - badDevotion) / (goodDevotion + badDevotion) * maxScaleGround;
                scaleAmount = (1 - ((MAX_ENCOUNTER - timesEncounter) * forgetDevotion + forgetCounter) / 
                    (forgetDevotion * MAX_ENCOUNTER)) * maxScaleGround;
                if (scaleAmount > 0)
                {
                    villageGround.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
                }
            }
			if(timesEncounter > 1){
				doublePositiveWorship.GetComponent<Renderer>().enabled = true;
				Color c = doublePositiveWorship.GetComponent<Renderer>().material.color;
				c.a = devotionCounter / devotionIncrement;
				doublePositiveWorship.GetComponent<Renderer>().material.color = c;
			} else if(timesEncounter == 1) {
				positiveWorship.GetComponent<Renderer>().enabled = true;
				Color c = positiveWorship.GetComponent<Renderer>().material.color;
				c.a = devotionCounter / devotionIncrement;
				positiveWorship.GetComponent<Renderer>().material.color = c;
			}
        }
        else
        {
            audioSource.Stop();
            Destroy(villageGround);
        }
		int houseCount = 0;
		int burningHouses = 0;
		for(int i=0; i < houses.Length; i++){
			houseCount++;
			if(houses[i] == null){
				cleanUpHouses();
				// burn other house
				if(houses.Length != 0){
					houses[Random.Range(0,houses.Length)].GetComponent<HouseScript>().isburning = true;
				}
				Update();
				return;
			}
			else if(timesEncounter < 1 && houses[i].GetComponent<HouseScript>().isburning){
				state = VillageState.BURNING;
				timesEncounter = 1;
				forgetCounter = 0;
			} else if(houses[i].GetComponent<HouseScript>().isburning){
				burningHouses++;
			} else if(state == VillageState.WORSHIPPING){
				houses[i].GetComponent<HouseScript>().isWorshipping = true;
			} else {
				houses[i].GetComponent<HouseScript>().isWorshipping = false;
			}
		}
		// Interacting with the lake
		if(lake != null){
			if(lake.GetComponent<LakeScript>().isFilled){
				houseCounter += Time.deltaTime;
				/*if(state == VillageState.NUETRAL && houseCount > 0){
					state = VillageState.WORSHIPPING;
					timesEncounter = 1;
					forgetCounter = 0;
				} else */if(state == VillageState.NUETRAL && wasFilling){
					state = VillageState.WORSHIPPING;
					timesEncounter = 1;
					forgetCounter = 0;
                    wasFilling = false;
				}
				if(houseCount < maxHouses && houseCounter > makeHouseSeconds && state == VillageState.WORSHIPPING){
					houseCounter = 0;
					Debug.Log("About to make house");
					// make new House
					makeNewHouse(lake);
				}
			}else if(lake.GetComponent<LakeScript>().isFilling && state == VillageState.NUETRAL){
				wasFilling = true;
			}
		} else{
			//throw new ExitGUIException();
		}


		if(updatedDevotion){
            //Pop up text here

			textCounter+=Time.deltaTime;
		}
		if(textCounter > textSeconds){
			textCounter = 0;
            updatedDevotion = false;

		}

		if(devotionCounter > devotionIncrement ){
			devotionCounter = 0;
			updatedDevotion = true;
			Debug.Log("OMG ITS DEVOTION UPDATE");
			if(state == VillageState.BURNING){
				pointsGained = devotionRateBad * houseCount * timesEncounter * burningHouses;
				badDevotion += (int)pointsGained;
			}
			else if(state == VillageState.WORSHIPPING){
				pointsGained = devotionRateGood * houseCount * timesEncounter;
				goodDevotion += (int)pointsGained;
			} else{
				Debug.Log("ERROR: THIS IS BAD");
			}
		}

		if(forgetCounter > forgetDevotion){
			timesEncounter-- ;
			Debug.Log(timesEncounter);
			timesEncounter = Mathf.Max(timesEncounter,0);
			forgetCounter = 0;
			if(timesEncounter < 1){
				state = VillageState.NUETRAL;
			}
		}

		//interact with lava flow nearby
		//NOTE: This only works when max_encounter is 2
		GameObject lavaHead = CameraControl.myPlay;
		if(lavaHead != null){
			if(nearby(lavaHead.transform.position, radiusNear)){
				if(state != VillageState.NUETRAL){
					alertBubble.GetComponent<Renderer>().enabled = true;
					timesEncounter++;
					timesEncounter = Mathf.Min(timesEncounter, MAX_ENCOUNTER);
					forgetCounter = 0;
                }
                else if(lake != null && lake.GetComponent<LakeScript>().isFilled)
                {
                    /*state = VillageState.WORSHIPPING;
                    timesEncounter = 1;
                    forgetCounter = 0;*/
                }
			}
		}

        //Color color = villageGround.GetComponent<SpriteRenderer>().color;
	}

	private bool nearby(Vector3 lavaPos, float radius){
		if(transform.position.x + radius > lavaPos.x && transform.position.x - radius < lavaPos.x){
			if(transform.position.y + radius > lavaPos.y && transform.position.y - radius < lavaPos.y){
				return true;
			}
		}
		return false;
	}

	private void makeNewHouse(GameObject lake){
		bool generate = true;
		Debug.Log ("Making new House");

		while(generate){
			//Vector3 lakePos = lake.transform.position;
            float xBound = lake.GetComponent<SpriteRenderer>().bounds.size.x;
            float yBound = lake.GetComponent<SpriteRenderer>().bounds.size.y;
			float x = Random.Range (0f, xBound);
            float y = Random.Range(0f, yBound);
			Vector2 newPos = new Vector2(lake.transform.position.x -xBound / 2+ x, lake.transform.position.y - yBound / 2 + y);
			house.transform.position = newPos;
			generate = false;
			Debug.Log("Trying to generate");
			//TODO figure out why this freezes the program and fix it so houses do not stack
			/*foreach(GameObject h in houses){
				if(testPoint(house,h)){
					generate = true;
				}
			}*/
		}
		GameObject newHouse = Instantiate (house) as GameObject;
		Debug.Log ("Done making house");

		newHouse.transform.parent = this.transform;
        //TODO: This only works if houses only spawn when there exists a lake
        newHouse.transform.rotation = lake.transform.rotation;
		GameObject[] moreHouses = new GameObject[houses.Length + 1];
		houses.CopyTo(moreHouses,0);
		houses = moreHouses;
		houses [houses.Length - 1] = newHouse;
		Debug.Log ("done adding house");
	}

	private bool testPoint(GameObject house, GameObject existingHouse){
        Vector3 sizeHouse = existingHouse.GetComponent<SpriteRenderer>().bounds.size;
		//right of it
        if (house.transform.position.x > existingHouse.transform.position.x + sizeHouse.x)
        {
			return true;
		}
		// Below it
        if (house.transform.position.y < existingHouse.transform.position.y - sizeHouse.y)
        {
			return true;
		}
		//to the left of it
        if (house.transform.position.x + sizeHouse.x > existingHouse.transform.position.x)
        {
			return true;
		}
		// Above it
        if (house.transform.position.y - sizeHouse.y > existingHouse.transform.position.y)
        {
			return true;
		}
		return false;
	}

	/**
	 * Cleans up the houses and resets wasFilling
	 */
	public void cleanUpHouses(){
		wasFilling = false;
		int houseCount = 0;
		GameObject[] newHouses = new GameObject[houses.Length-1];
		for(int i=0; i < houses.Length; i++){
			if(houses[i] != null){
				newHouses[houseCount] = houses[i];
			} else { 
				houseCount--;
			}
			houseCount++;
		}
		houses = newHouses;
	}

    /*void OnGUI(){
        if(textCounter <= textSeconds && textCounter!=0){
            updatedDevotion = false;
            GUIStyle style = new GUIStyle();
            style.fontSize = 18;
            GUIText text = new GUIText();
            text.text = pointsGained.ToString();
            text.transform.position = FindObjectOfType<Camera>().WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + textCounter));
            //(pointsRect, pointsGained.ToString(), style);

        }
    }*/
}
