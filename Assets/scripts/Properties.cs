using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    /**
     * Add more keys here that need to be persistent here.
     * Takes the format of 'key=value'
     * */
    class Properties : MonoBehaviour
    {
        public static string PLAY_STYLE = "play_style";
        public enum PlayStyle{GOOD,BAD,NUETRAL};
        public static PlayStyle lastPlayedStyle = PlayStyle.NUETRAL;
        public static float lastScore = 0f;
        public static string fileName = "test";

        void OnDestroy()
        {
            PlayerPrefs.SetString(PLAY_STYLE, lastPlayedStyle.ToString());
        }
    }
}
