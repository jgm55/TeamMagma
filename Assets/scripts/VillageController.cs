using UnityEngine;
using System.Collections;

public class VillageController : MonoBehaviour {
	
	public enum VillageState{BURNING, NUETRAL, WORSHIPPING};
	public GameObject lake;
	public GameObject[] houses;
	public GameObject house;

	public int devotion = 0;
	int devotionRateBad = 4;
	int devotionRateGood = 1;
	int forgetDevotion = 60;
	int maxHouses = 6;

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
		if(state != VillageState.NUETRAL){
			devotionCounter += Time.deltaTime;
			forgetCounter += Time.deltaTime;
		}
		int houseCount = 0;
		for(int i=0; i < houses.Length; i++){
			houseCount++;
			if(houses[i] == null){
				houseCount--;
			}
			else if(timesEncounter < 1 && houses[i].GetComponent<HouseScript>().isburning){
				state = VillageState.BURNING;
				timesEncounter = 1;
				forgetCounter = 0;
			}
		}
		if(lake != null){
			if(lake.GetComponent<LakeScript>().isFilled){
				houseCounter += Time.deltaTime;
				if(houseCount < maxHouses && state == VillageState.WORSHIPPING 
				   && houseCounter > makeHouseSeconds){
					houseCounter = 0;
					// make new House
					//makeNewHouse(lake, houses);
				}
			} else if(lake.GetComponent<LakeScript>().isFilling && state == VillageState.NUETRAL){
				state = VillageState.WORSHIPPING;
				timesEncounter = 1;
				forgetCounter = 0;
			}
		} else{
			throw new ExitGUIException();
		}


		//TODO add in if nearby then up timesEncounter and reset forgetCounter


		if(devotionCounter > devotionIncrement ){
			devotionCounter = 0;
			Debug.Log("OMG ITS DEVOTION UPDATE");
			if(state == VillageState.BURNING){
				devotion += devotionRateBad * houseCount * timesEncounter;
			}
			else if(state == VillageState.WORSHIPPING){
				devotion += devotionRateGood * houseCount * timesEncounter;
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
	}

	private void makeNewHouse(GameObject lake, GameObject[] houses){
		bool generate = true;
		Debug.Log ("Making new House");

		while(generate){
			Vector3 lakePos = lake.transform.position;
			float x = Random.Range (0f, lake.transform.localScale.x);
			float y = Random.Range (0f, lake.transform.localScale.y);
			Vector2 newPos = new Vector2(lake.transform.position.x + x, lake.transform.position.y + y);
			house.transform.position = newPos;
			generate = false;

			foreach(GameObject h in houses){
				if(testPoint(house,h)){
					generate = true;
				}
			}
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
		} else {

		}
		// Below it
		if(house.transform.position.y < existingHouse.transform.position.y - existingHouse.transform.localScale.y){
			return true;
		} else {
			return false;
		}
	}
}
