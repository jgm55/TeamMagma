using UnityEngine;
using System.Collections;
using Assets.scripts;

public class TitleScreenButtonController : MonoBehaviour {

    public Sprite good;
    public Sprite bad;
    public string sceneName;
    public GameObject matsya;

    private int count = 0;
	// Use this for initialization
	void Start () {
        if (Properties.lastPlayedStyle == Properties.PlayStyle.BAD)
        {
            GetComponent<SpriteRenderer>().sprite = bad;
        }
        else if (Properties.lastPlayedStyle == Properties.PlayStyle.GOOD)
        {
            GetComponent<SpriteRenderer>().sprite = good;
        }
	}

    void OnMouseDown()
    {
        Application.LoadLevel(sceneName);
        count++;
        if (count >= 5)
        {
            //GetComponent<SpriteRenderer>().sprite = matsya;
            Instantiate(matsya);
        } if (count >= 6)
        {
            Application.LoadLevel("StartScreen");

        }
    }
}
