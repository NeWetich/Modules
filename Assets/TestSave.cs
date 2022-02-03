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

    SaveData datasave = new SaveData();

    public GameObject fullScreenClick;
    public GameObject volumeClick;

    public void ChangeVolume(float val)
    {
        datasave.volume = val;
    }

    public void ChangeFullscreenMode(bool val)
    {
        datasave.fullScreen = val;
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
