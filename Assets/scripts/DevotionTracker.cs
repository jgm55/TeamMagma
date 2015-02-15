using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DevotionTracker : MonoBehaviour {

	private float devotion = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		devotion = GameObject.FindGameObjectWithTag ("volcano").GetComponent<VolcanoController> ().devotion;
	}

	void OnGUI(){
		Text text = GetComponent<Text>();
		text.text = "Score: " + devotion.ToString();
	}
}
