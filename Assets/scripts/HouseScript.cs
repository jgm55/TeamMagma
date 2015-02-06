using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

	public float burnTime = 10;
	public bool isburning = false;
	private float counter = 0f;
	public Sprite burningSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isburning){
			GetComponent<SpriteRenderer>().sprite = burningSprite;
			if(counter >= burnTime){
				Destroy(this.gameObject);
			}
			counter += Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		isburning = true;
	}
}
