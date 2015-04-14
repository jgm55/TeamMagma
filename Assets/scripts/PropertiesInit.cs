using UnityEngine;
using System.Collections;
using Assets.scripts;
using System;


public class PropertiesInit : MonoBehaviour {
    void Start()
    {
        string filePath = Application.persistentDataPath + Properties.fileName;
        if (System.IO.File.Exists(filePath))
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] splitLine = line.Split('=');
                if (splitLine[0] == Properties.lastPlayedStyleKey)
                {
                    Properties.lastPlayedStyle = (Properties.PlayStyle)Enum.Parse(typeof(Properties.PlayStyle), splitLine[1]);
                }
            }
        }
    }
}
