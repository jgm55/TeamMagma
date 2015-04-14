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
        public enum PlayStyle{GOOD,BAD,NUETRAL};
        public static PlayStyle lastPlayedStyle = PlayStyle.NUETRAL;
        public static float lastScore = 0f;
        public static string fileName = "test";
        public static string lastPlayedStyleKey = "lastPlayed";

        void OnDestroy()
        {
            string filePath = Application.persistentDataPath + fileName;
            string[] lines = { lastPlayedStyleKey + "=" + lastPlayedStyle.ToString() };
            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}
