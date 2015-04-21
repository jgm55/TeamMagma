using UnityEngine;
using System.Collections;
using Assets.scripts;
using System;


public class PropertiesInit : MonoBehaviour {
    void Start()
    {
        string playStyle = PlayerPrefs.GetString(Properties.PLAY_STYLE);
        if (playStyle != "")
        {
            Properties.lastPlayedStyle = (Properties.PlayStyle)Enum.Parse(typeof(Properties.PlayStyle), playStyle);
        }
    }
}
