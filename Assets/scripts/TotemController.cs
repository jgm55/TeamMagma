using UnityEngine;
using System.Collections;
using Assets.scripts;

public class TotemController : MonoBehaviour {

    Animator animator;
	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Properties.lastPlayedStyle == Properties.PlayStyle.BAD){
            Debug.Log("updating bad animation");
            animator.SetBool("isAngry", true);
        } else if(Properties.lastPlayedStyle == Properties.PlayStyle.GOOD){
            Debug.Log("updating good animation");
            animator.SetBool("isHappy", true);
        } else {
            Debug.Log("updating nuetral animation");
            animator.SetBool("isNuetral", true);
        }
	}
}
