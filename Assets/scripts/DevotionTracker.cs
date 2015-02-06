using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DevotionTracker : MonoBehaviour {

	private int devotion = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		devotion = 0;
		VillageController[] villages = FindObjectsOfType<VillageController> ();
		foreach(VillageController village in villages){
			devotion += village.devotion;
		}
	}

	void OnGUI(){
		Text text = GetComponent<Text>();
		text.text = "Score: " + devotion.ToString();
	}
}
