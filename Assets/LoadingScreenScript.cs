using UnityEngine;
using System.Collections;

public class LoadingScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.LoadLevelAsync("MainGame");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
