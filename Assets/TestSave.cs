using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.UI;


public class TestSave : MonoBehaviour
{

    [System.Serializable]
    public class SaveData
    {
        public bool fullScreen = false;
    }

    void Start()
    {
        Encryption.AES.Encrypt(new byte[10], "pass");
    }

    SaveData datasave = new SaveData();

    public string save = "/SaveData.dat";

    public Toggle fullScreenClick;

    public void SaveSettings()
    {
        Screen.fullScreen = datasave.fullScreen;
    }

        public void ClickSave()
    {
        SaveSerial.SaveGame(datasave);
    }
 
    public void ClickLoad()
    {
        SaveSerial.LoadGame<SaveData>();
    }

}
