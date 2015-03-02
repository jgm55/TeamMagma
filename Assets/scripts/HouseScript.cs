using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

	public float burnTime = 10;
	public bool isburning = false;
	public bool isWorshipping = false;

	private float counter = 0f;
	public Sprite[] burningSprites = new Sprite[24];
	public Sprite[] worshippingSprites = new Sprite[24];

	private int animCounter = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isburning){
			isWorshipping = false;
			GetComponent<SpriteRenderer>().sprite = burningSprites[animCounter];
			if(counter >= burnTime){
				Destroy(this.gameObject);
			}
			counter += Time.deltaTime;
			animCounter++;
			if(animCounter >= burningSprites.Length){
				animCounter = 0;
			}
		}
		if(isWorshipping){
			GetComponent<SpriteRenderer>().sprite = worshippingSprites[animCounter];
			animCounter++;
			if(animCounter >= worshippingSprites.Length){
				animCounter = 0;
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		isburning = true;
	}
}
