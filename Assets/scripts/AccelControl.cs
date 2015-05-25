using UnityEngine;
using System.Collections;

public class AccelControl : MonoBehaviour {

	float scaler = 3;
	public float maxVelocity = 1.2f;

	//public Vector3 friction = new Vector3 (.5,.5,0);
	public Vector3 dir;
	public GUIStyle style;
    //TODO fix this
	public float timeLava;
	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera();
	}

	void awake(){
		style=new GUIStyle();
		style.alignment=TextAnchor.MiddleCenter;
		style.normal.textColor=Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		dir = Input.acceleration;
		dir.z = 0;
		if(dir.y > 0){
			dir.y = 0;
		}
		//TODO Make this actually limit by the maxVelocity vector, not both individually
		if(GetComponent<Rigidbody2D>().velocity.x > maxVelocity && dir.x > 0){
			dir.x = 0;
		} else if(GetComponent<Rigidbody2D>().velocity.x < -1*maxVelocity && dir.x < 0){
			dir.x = 0;
		}
		if(GetComponent<Rigidbody2D>().velocity.y > maxVelocity && dir.y > 0){
			dir.y = 0;
		} else if(GetComponent<Rigidbody2D>().velocity.y < -1*maxVelocity && dir.y < 0){
			dir.y = 0;
		}
		dir *= scaler;
		GetComponent<Rigidbody2D>().AddForce(dir);

		timeLava -= Time.deltaTime;
		if ( timeLava < 0 )
		{
			//TODO Fix the lava path
			GameObject.FindGameObjectWithTag("volcano").GetComponent<VolcanoController>().instantiated = false;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().Reset();
			Component[] components = this.GetComponents<Component>();
			
            //Do raytracing to see if inside a lake
            Vector2 pos = Camera.main.ScreenToWorldPoint(this.transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, pos);
//            Debug.Log("hits: " + hits);
            foreach(RaycastHit2D hit in hits){
                Debug.Log("hit" + hit.collider.gameObject);
                LakeScript lake = hit.collider.gameObject.GetComponent<LakeScript>();
                if (lake != null)
                {
                    lake.OnTriggerExit2D(this.GetComponent<Collider2D>());
                }
            }

			Destroy(this.gameObject);
		}

	}

	/*void OnCollisionEnter2D(Collision2D collision) {
		HouseScript houseScript = collision.collider.gameObject.GetComponent<HouseScript> ();
		if(houseScript != null){
			houseScript.Burn();
		}
		LakeScript lakeScript = collision.collider.gameObject.GetComponent<LakeScript> ();
		if(lakeScript != null){
			lakeScript.Fill();
		}
		Destroy(collision.collider);
	}*/

	void OnGUI()
	{
		//GUI.Label(new Rect(Screen.width/2-100,Screen.height/2-100,200,200),dir.ToString(),style);
		//GUI.Label(new Rect(Screen.width/2-100,Screen.height/2-100,220,210),rigidbody2D.velocity.ToString(),style);

	}
}
