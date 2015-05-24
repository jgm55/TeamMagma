using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GUIController : MonoBehaviour {
	
	public Image goodGUI;
	public Image badGUI;
	public Bloom goodBloom;
	public Image grunge;
	
	void OnGUI () {
		if(Properties.lastPlayedStyle == Properties.PlayStyle.BAD){
			badGUI.enabled = true;
			goodGUI.enabled = false;
			goodBloom.enabled = false;
			grunge.enabled = true;
		} else if(Properties.lastPlayedStyle == Properties.PlayStyle.GOOD){
			badGUI.enabled = false;
			goodGUI.enabled = true;
			goodBloom.enabled = true;
			grunge.enabled = false;
		} else {
			badGUI.enabled = false;
			goodGUI.enabled = false;
			goodBloom.enabled = false;
			grunge.enabled = false;
		}
	}
}
