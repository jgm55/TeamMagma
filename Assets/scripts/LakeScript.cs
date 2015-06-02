using UnityEngine;
using System.Collections;

public class LakeScript : MonoBehaviour {

	float fillTime = 4.4f;
	public bool isFilling = false;
	public bool isFilled = false;
	private float counter = 0f;
	private float lakeChangeSeconds;
	private int lakeIndex = 0;
    public AudioSource audioSource;
    Animator animator;
   
    float startVelocity;

	float startDrag = 1;
	float lakeDrag = 2;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isFilling){
			if(counter >= fillTime){
				isFilled = true;
				isFilling = false;
			}

            animator.SetBool("isFilling",true);

			counter += Time.deltaTime;
        }
        else
        {
            //animator.SetBool("isFilling", false);
        }
	}

    public void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("isFilling", false);
		isFilling = false;
		other.gameObject.GetComponent<Rigidbody2D>().drag = startDrag;
		AccelControl script = other.GetComponent<AccelControl>();
		script.maxVelocity = startVelocity;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(.01f);
        audioSource.Stop();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(!isFilled){
            animator.SetBool("isFilling", true);
            audioSource.Play();
			isFilling = true;
			other.gameObject.GetComponent<Rigidbody2D>().drag = lakeDrag;
			AccelControl script = other.GetComponent<AccelControl>();
			startVelocity = script.maxVelocity;
			script.maxVelocity = startVelocity / 2;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().startShakingCamera(.01f);
		}
	}
}
