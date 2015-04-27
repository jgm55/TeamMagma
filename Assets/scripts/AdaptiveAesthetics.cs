using UnityEngine;
using System.Collections;
using Assets.scripts;

public class AdaptiveAesthetics : MonoBehaviour {

    public GameObject audioPrefab;

    AudioSource audioSource;
    public AudioClip badSound;
    public AudioClip goodSound;
    public AudioClip nuetralSound;

    Properties.PlayStyle last = Properties.PlayStyle.NUETRAL;

	// Use this for initialization
	void Start () {
        GameObject audioObj = Instantiate(audioPrefab) as GameObject;
        audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.PlayOneShot(nuetralSound);
	}
	
	// Update is called once per frame
	void Update () {
        if (last != Properties.lastPlayedStyle)
        {
            if (Properties.lastPlayedStyle == Properties.PlayStyle.BAD)
            {
                Destroy(audioSource);
                GameObject audioObj = Instantiate(audioPrefab) as GameObject;
                audioSource = audioObj.GetComponent<AudioSource>();
                audioSource.PlayOneShot(badSound);
                last = Properties.lastPlayedStyle;
            }
            else if (Properties.lastPlayedStyle == Properties.PlayStyle.GOOD)
            {
                Destroy(audioSource);
                GameObject audioObj = Instantiate(audioPrefab) as GameObject;
                audioSource = audioObj.GetComponent<AudioSource>();
                audioSource.PlayOneShot(goodSound);
                last = Properties.lastPlayedStyle;
            }
            else
            {
                Destroy(audioSource);
                GameObject audioObj = Instantiate(audioPrefab) as GameObject;
                audioSource = audioObj.GetComponent<AudioSource>();
                audioSource.PlayOneShot(nuetralSound);
                last = Properties.lastPlayedStyle;
            }
        }
	}
}
