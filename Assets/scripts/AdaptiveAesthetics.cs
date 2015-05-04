using UnityEngine;
using System.Collections;
using Assets.scripts;

public class AdaptiveAesthetics : MonoBehaviour {

    public GameObject audioPrefab;

    AudioSource audioSource;
    public AudioClip badSound;
    public AudioClip goodSound;
    public AudioClip nuetralSound;

    float SOURCE_VOLUME = .35f;

    Properties.PlayStyle last = Properties.PlayStyle.NUETRAL;

	// Use this for initialization
	void Start () {
        audioSource = setUpAudioSource();
        audioSource.PlayOneShot(nuetralSound);
	}
	
	// Update is called once per frame
	void Update () {
        if (last != Properties.lastPlayedStyle)
        {
            if (Properties.lastPlayedStyle == Properties.PlayStyle.BAD)
            {
                audioSource = setUpAudioSource();
                audioSource.PlayOneShot(badSound);
                last = Properties.lastPlayedStyle;
            }
            else if (Properties.lastPlayedStyle == Properties.PlayStyle.GOOD)
            {
                audioSource = setUpAudioSource();
                audioSource.PlayOneShot(goodSound);
                last = Properties.lastPlayedStyle;
            }
            else
            {
                audioSource = setUpAudioSource();
                audioSource.PlayOneShot(nuetralSound);
                last = Properties.lastPlayedStyle;
            }
        }
	}

    private AudioSource setUpAudioSource()
    {
        Destroy(audioSource);
        GameObject audioObj = Instantiate(audioPrefab) as GameObject;
        audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.volume = SOURCE_VOLUME;
        return audioSource;
    }
}
