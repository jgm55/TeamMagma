using UnityEngine;
using System.Collections;
using Assets.scripts;


public class GUIController : MonoBehaviour {

	public Texture2D goodGUI;
	public Texture2D badGUI;

	Properties.PlayStyle last = Properties.PlayStyle.NUETRAL;


	void OnGUI () {
		/*if(Properties.lastPlayedStyle == Properties.PlayStyle.BAD){
			GUI.DrawTexture(new Rect(1,1,0,.934), badGUI, ScaleMode.ScaleToFit, true, 10.0f);
			Debug.Log("updating bad GUI");
			("isAngry", true);
		} else if(Properties.lastPlayedStyle == Properties.PlayStyle.GOOD){
			GUI.DrawTexture(Rect(1,1,0,.934), goodGUI, ScaleMode.ScaleToFit, true, 10.0f);
			Debug.Log("updating good GUI");
			("isHappy", true);
		} else {
			Debug.Log("updating nuetral GUI");
			("isNuetral", true);
		}*/
	}
}
