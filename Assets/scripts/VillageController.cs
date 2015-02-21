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

	public int goodDevotion = 0;
	public int badDevotion = 0;
	int devotionRateBad = 4;
	int devotionRateGood = 1;
	int forgetDevotion = 60;
	int maxHouses = 6;
	float radiusNear = 6.0f;
	int MAX_ENCOUNTER = 2;

	private float forgetCounter = 0;
	private float devotionCounter = 0;
	private float houseCounter = 0;
	private int timesEncounter = 0;
	
	VillageState state = VillageState.NUETRAL;

	int devotionIncrement = 10;
	int makeHouseSeconds = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(houses.Length == 0){
			state = VillageState.NUETRAL;
		}

		if(state != VillageState.NUETRAL){
			devotionCounter += Time.deltaTime;
			forgetCounter += Time.deltaTime;
		} else {
			positiveWorship.renderer.enabled = false;
			negativeWorship.renderer.enabled = false;
			doublePositiveWorship.renderer.enabled = false;
			doubleNegativeWorship.renderer.enabled = false;
			
		}
		alertBubble.renderer.enabled = false;

		if(state == VillageState.BURNING){
			if(timesEncounter > 1){
				doubleNegativeWorship.renderer.enabled = true;
			} else {
				negativeWorship.renderer.enabled = true;
			}
		} else if(state == VillageState.WORSHIPPING){
			if(timesEncounter > 1){
				doublePositiveWorship.renderer.enabled = true;
			} else {
				positiveWorship.renderer.enabled = true;
			}
		}
		int houseCount = 0;
		for(int i=0; i < houses.Length; i++){
			houseCount++;
			if(houses[i] == null){
				cleanUpHouses();
				// burn other house
				if(houses.Length !=0){
					houses[Random.Range(0,houses.Length)].GetComponent<HouseScript>().isburning = true;
				}
				Update();
				return;
			}
			else if(timesEncounter < 1 && houses[i].GetComponent<HouseScript>().isburning){
				state = VillageState.BURNING;
				timesEncounter = 1;
				forgetCounter = 0;
			}
		}
		// Interacting with the lake
		if(lake != null){
			if(lake.GetComponent<LakeScript>().isFilled){
				houseCounter += Time.deltaTime;
				if(houseCount < maxHouses && state == VillageState.WORSHIPPING 
				   && houseCounter > makeHouseSeconds){
					houseCounter = 0;
					Debug.Log("About to make house");
					// make new House
					makeNewHouse(lake);
				}
			} else if(lake.GetComponent<LakeScript>().isFilling && state == VillageState.NUETRAL){
				state = VillageState.WORSHIPPING;
				timesEncounter = 1;
				forgetCounter = 0;
			}
		} else{
			throw new ExitGUIException();
		}


		if(devotionCounter > devotionIncrement ){
			devotionCounter = 0;
			Debug.Log("OMG ITS DEVOTION UPDATE");
			if(state == VillageState.BURNING){
				badDevotion += devotionRateBad * houseCount * timesEncounter;
			}
			else if(state == VillageState.WORSHIPPING){
				goodDevotion += devotionRateGood * houseCount * timesEncounter;
			} else{
				Debug.Log("ERROR: THIS IS BAD");
			}
		}

		if(forgetCounter > forgetDevotion){
			timesEncounter-- ;
			Debug.Log("OMG ITS HERE");
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
					alertBubble.renderer.enabled = true;
					timesEncounter++;
					timesEncounter = Mathf.Min(timesEncounter, MAX_ENCOUNTER);
					forgetCounter = 0;
				}
			}
		}
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
			float x = Random.Range (0f, lake.transform.localScale.x);
			float y = Random.Range (0f, lake.transform.localScale.y);
			Vector2 newPos = new Vector2(lake.transform.position.x + x, lake.transform.position.y + y);
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
		GameObject newHouse = (GameObject)Instantiate (house);
		Debug.Log ("Done making house");

		GameObject[] moreHouses = new GameObject[houses.Length + 1];
		houses.CopyTo(moreHouses,0);
		houses = moreHouses;
		houses [houses.Length - 1] = newHouse;
		Debug.Log ("done adding house");
	}

	private bool testPoint(GameObject house, GameObject existingHouse){
		//right of it
		if(house.transform.position.x > existingHouse.transform.position.x + existingHouse.transform.localScale.x){
			return true;
		}
		// Below it
		if(house.transform.position.y < existingHouse.transform.position.y - existingHouse.transform.localScale.y){
			return true;
		}
		//to the left of it
		if(house.transform.position.x + existingHouse.transform.localScale.x > existingHouse.transform.position.x){
			return true;
		}
		// Above it
		if(house.transform.position.y - existingHouse.transform.localScale.y > existingHouse.transform.position.y){
			return true;
		}
		return false;
	}

	/**
	 * 
	 */
	public void cleanUpHouses(){
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

}
