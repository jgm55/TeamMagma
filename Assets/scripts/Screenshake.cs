using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Screenshake : MonoBehaviour {

	public Camera mainCamera;
	float shake =2f;
	float shakeAmt =0.7f;
	float decreaseFactor = 1.0f;

		void Update() {
	
			if (shake > 0) {
			mainCamera.transform.localPosition = Random.insideUnitSphere * shakeAmt;
			shake -= Time.deltaTime * decreaseFactor;
			Debug.Log("shaking");
		
			} else {
				shake = 0;
					}
		
			}

	}