using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public GameObject[] levels;
    public GameObject baseVillage;
    System.TimeSpan[] timeSpans;

    int levelNumber = 0;
    int variance = 1;
    public GameObject currentLevel;

    int targetDifficulty = 1;

    System.DateTime startTime = System.DateTime.Now;
    System.DateTime endTime;
    bool erupting = false;

    const int DIFFICULTY_SWING = 5;

    void Start()
    {
        //TODO make this not just 60 seconds for each
        timeSpans = new System.TimeSpan[levels.Length];
        System.TimeSpan span0 = new System.TimeSpan(0, 0, 50);
        System.TimeSpan span1 = new System.TimeSpan(0, 0, 45);
        System.TimeSpan span2 = new System.TimeSpan(0, 0, 50);
        System.TimeSpan span3 = new System.TimeSpan(0, 0, 40);
        System.TimeSpan span4 = new System.TimeSpan(0, 0, 60);

        timeSpans[0] = span0;
        timeSpans[1] = span1;
        timeSpans[2] = span2;
        timeSpans[3] = span3;
        timeSpans[4] = span4;
    }


    /**
     * Returns true if the game is out of levels. False otherwise
     * */
    public bool erupt(){
        if (erupting)
        {
            return false;
        }
        erupting = true;
        endTime = System.DateTime.Now;
        System.TimeSpan difference = endTime - startTime;
        //expected time - their time
        int timeDifferenceSeconds = (int)(timeSpans[levelNumber].TotalSeconds - difference.TotalSeconds);
        targetDifficulty += timeDifferenceSeconds / (int)((timeSpans[levelNumber].TotalSeconds / DIFFICULTY_SWING));
        increaseStats(targetDifficulty);
        levelNumber++;
        Debug.Log("ERUPT: " + levelNumber);
        Debug.Log("ERUPTION: " + levels.Length);
        if (levelNumber == levels.Length)
        {
            //GAME IS OVER
            Debug.Log("GAME IS OVER");
            return true;
        }
        
        //Make new level and update reference
        GameObject nextLevel = Instantiate(levels[levelNumber]) as GameObject;
        for (int i = 0; i < currentLevel.transform.childCount;i++)
        {
            Transform trans = currentLevel.transform.GetChild(i);
            trans.SetParent(nextLevel.transform);
        }
        Destroy(currentLevel);
        currentLevel = nextLevel;

        //Get all positions in new Level
        GameObject[] villages = GameObject.FindGameObjectsWithTag("villagePosition");
        
        float toSpawn = levelNumber + 2;

        foreach (GameObject village in villages)
        {
            if ((float)Random.Range(0, villages.Length) / villages.Length <= toSpawn / villages.Length)
            {
                Debug.Log("village: " + village);
                int currentVillageLevel = -2;

                //First choose distance
                int distance = village.GetComponent<VillagePosition>().position;
                currentVillageLevel = distance * 2 - 2;//minus 2 is for lake
                GameObject newVillage = Instantiate(baseVillage, village.transform.position, village.transform.rotation) as GameObject;
                newVillage.transform.SetParent(currentLevel.transform);
                //Then choose if lake or note factoring in average village expected size with some variance
                if (currentVillageLevel + 3 < targetDifficulty + Random.Range(-1 * variance, variance+1))
                {
                    // remove lake
                    Destroy(newVillage.GetComponent<VillageController>().lake);
                    currentVillageLevel += 2;
                    Debug.Log("lake removed");
                }

                //finally finish with number of houses to get close to number
                int housesToRemove = targetDifficulty - currentVillageLevel + Random.Range(-1 * variance, variance+1);
                if (housesToRemove > 5)
                {
                    housesToRemove = 5;// set min to be one house
                }
                Debug.Log("housesToRemove "+ housesToRemove);
                while (housesToRemove > 0)
                {
                    housesToRemove--;
                    newVillage.GetComponent<VillageController>().removeRandomHouse();
                }
                Destroy(village);
            }
        }
        Debug.Log("DoneErupt");
        startTime = System.DateTime.Now;
        erupting = false;
        return false;
    }

    private void increaseStats(int targetDifficulty)
    {
        VolcanoController volcano = FindObjectOfType<VolcanoController>();
        float amountToScale = Mathf.Max(1,targetDifficulty / (float)(targetDifficulty - 1));
        volcano.maxSpeedStart *= amountToScale;
        volcano.maxSpeedBad *= amountToScale;
        volcano.timeLavaStart *= amountToScale;
        volcano.timeGoodLava *= amountToScale;
    }

}
