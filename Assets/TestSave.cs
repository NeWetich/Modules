using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class TestSave : MonoBehaviour
{

    void Start()
    {
        Encryption.AES.Encrypt(new byte[10], "pass");
        //GameObject.Find("SaveData").GetComponent<SaveData>();
    }

    SaveData datasave = new SaveData();

    public string save = "/SaveData.dat";

    public Button saved;
    public Button loaded;

    public void ClickSave(bool datasave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + save);
        bf.Serialize(file, datasave);
        file.Close();
        Debug.Log("Game data saved!");

    }

    public void ClickLoad()
    {

    }

    public bool ForClickLoad()
    {
        if (!File.Exists(Application.persistentDataPath + save))
        {
            Debug.LogError("There is no save data!");
            return default(bool);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + save, FileMode.Open);
        bool result = (bool)bf.Deserialize(file);
        file.Close();
        Debug.Log("Game data loaded!");
        return result;
    }

}
