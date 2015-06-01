using UnityEngine;
using System.Collections;
using Assets.scripts;

public class PlantScript : MonoBehaviour {

    public Sprite nuetral;
    public Sprite good;
    public Sprite bad;

	// Use this for initialization
	void Start () {
	    
	}
    //TODO add more interaction?
	
	// Update is called once per frame
	void Update () {
        if (Properties.lastPlayedStyle == Properties.PlayStyle.BAD)
        {
            GetComponent<SpriteRenderer>().sprite = bad;
        }
        else if (Properties.lastPlayedStyle == Properties.PlayStyle.GOOD)
        {
            GetComponent<SpriteRenderer>().sprite = good;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = nuetral;
        }
	}
}
