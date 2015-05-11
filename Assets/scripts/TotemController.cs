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
            animator.SetBool("isAngry", true);
        } else if(Properties.lastPlayedStyle == Properties.PlayStyle.GOOD){
            animator.SetBool("isHappy", true);
        } else {
            animator.SetBool("isNuetral", true);
        }
	}
}
