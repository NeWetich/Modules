using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.UI;


public class SaveSerial : MonoBehaviour
{

    void Start()
    {
        Encryption.AES.Encrypt(new byte[10], "pass");
    }

    public static string save = "/SaveData.dat";

    public static void SaveGame(object datasave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + save);
        bf.Serialize(file, datasave);
        file.Close();
        Debug.Log("Game data saved!");

    }
    public static T LoadGame<T>()
    {
        if (!File.Exists(Application.persistentDataPath + save))
        {
            Debug.LogError("There is no save data!");
            return default(T);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + save, FileMode.Open);
        T result = (T)bf.Deserialize(file);
        file.Close();
        Debug.Log("Game data loaded!");
        return result;
    }

}