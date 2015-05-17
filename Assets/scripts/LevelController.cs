using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public GameObject[] levels;
    public GameObject[] goodVillagePrefabs;
    public GameObject[] badVillagePrefabs;

    int levelNumber = 0;
    public GameObject currentLevel;

	// Update is called once per frame
	void Update () {
	    
	}


    /**
     * Returns true if the game is out of levels. False otherwise
     * */
    public bool erupt(){
        levelNumber++;
        Debug.Log("ERUPT: " + levelNumber);
        if (levelNumber == levels.Length)
        {
            //GAME IS OVER
            return true;
        }
        GameObject nextLevel = Instantiate(levels[levelNumber]) as GameObject;
        Debug.Log("child " + currentLevel.transform.childCount);
        for (int i = 0; i < currentLevel.transform.childCount;i++)
        {
            Transform trans = currentLevel.transform.GetChild(i);
            trans.SetParent(nextLevel.transform);
        }
        Destroy(currentLevel);
        currentLevel = nextLevel;

        GameObject[] villages = GameObject.FindGameObjectsWithTag("villagePosition");

        foreach (GameObject village in villages)
        {
            Debug.Log("village: " + village);
            if (Random.Range(0, 2) == 1)
            { // Good village spawn
                GameObject obj = Instantiate(goodVillagePrefabs[Random.Range(0,goodVillagePrefabs.Length)],
                    village.transform.position, village.transform.rotation) as GameObject;
                obj.transform.SetParent(currentLevel.transform);
            }
            else
            { // bad village spawn
                GameObject obj = Instantiate(badVillagePrefabs[Random.Range(0, goodVillagePrefabs.Length)],
                    village.transform.position, village.transform.rotation) as GameObject;
                obj.transform.SetParent(currentLevel.transform);
            }
            Destroy(village);
        }

        Debug.Log("DoneErupt");
        return false;
    }

}
