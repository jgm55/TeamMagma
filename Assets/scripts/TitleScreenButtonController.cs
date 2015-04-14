using UnityEngine;
using System.Collections;
using Assets.scripts;

public class TitleScreenButtonController : MonoBehaviour {

    public Sprite good;
    public Sprite bad;
    public string sceneName;

	// Use this for initialization
	void Start () {
	    if(Properties.lastWin == Properties.PlayStyle.BAD){
            GetComponent<SpriteRenderer>().sprite = bad;
        }
        else if (Properties.lastWin == Properties.PlayStyle.GOOD)
        {
            GetComponent<SpriteRenderer>().sprite = good;
        }
	}

    void OnMouseDown()
    {
        Application.LoadLevel(sceneName);
    }
}
