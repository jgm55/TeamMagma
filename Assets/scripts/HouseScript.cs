using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

	float burnTime = 15;
	public bool isburning = false;
	public bool isWorshipping = false;

	private float counter = 0f;
	public Sprite[] burningSprites = new Sprite[24];
	public Sprite[] worshippingSprites = new Sprite[24];

	private Sprite original;
	private int animCounter = 0;

	// Use this for initialization
	void Start () {
		original = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().sprite = original;
		isWorshipping = this.GetComponentInParent<VillageController>().state == VillageController.VillageState.WORSHIPPING;
		if(isburning){
			isWorshipping = false;
			this.GetComponentInParent<VillageController>().state = VillageController.VillageState.BURNING;
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
