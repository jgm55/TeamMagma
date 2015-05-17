using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;


public class GUIController : MonoBehaviour {

	public Image goodGUI;
	public Image badGUI;
	

	void OnGUI () {
		if(Properties.lastPlayedStyle == Properties.PlayStyle.BAD){
			badGUI.enabled = true;
			goodGUI.enabled = false;
			Debug.Log("updating bad GUI");
		} else if(Properties.lastPlayedStyle == Properties.PlayStyle.GOOD){
			badGUI.enabled = false;
			goodGUI.enabled = true;
			Debug.Log("updating good GUI");
		} else {
			badGUI.enabled = false;
			goodGUI.enabled = false;
			Debug.Log("updating nuetral GUI");
		}
	}
}
