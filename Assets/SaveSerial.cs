using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//������ ����� � ������ ���� +
//������� �������������� ����� � ����� +
//SaveGame � LoadGame �������� +-
public class SaveSerial : MonoBehaviour
{
    [SerializeField] private Encryption ssilkaNaEncryption;

    [SerializeField] private ClassField ssilkaNaClassField;

    void Start()
    {
        ssilkaNaEncryption = GetComponent<Encryption>();
        ssilkaNaClassField = GetComponent<ClassField>();
    }

    void Update()
    {
        ssilkaNaEncryption.enabled = true;
        ssilkaNaClassField.enabled = true;
    }

    [System.Serializable]
    public class SaveData
    {
        string savedString;
        int savedInt;
        float savedFloat;
        bool savedBool;
    }//


    SaveData datasave = new SaveData();

    string save = "/SaveData.dat";


    string stringToSave;
    int intToSave;
    float floatToSave;
    bool boolToSave;

    //GUI 
    public void OnGUI() 
    {
        if (GUI.Button(new Rect(0, 0, 125, 50), "int "))
            intToSave++;
        if (GUI.Button(new Rect(0, 100, 125, 50), "float "))
            floatToSave += 0.1f;
        if (GUI.Button(new Rect(0, 200, 125, 50), "bool "))
            boolToSave = boolToSave ? boolToSave = false : boolToSave = true;

        stringToSave = GUI.TextField(new Rect(0, 300, 125, 25), stringToSave);

        GUI.Label(new Rect(375, 0, 125, 50), "int value " + intToSave);
        GUI.Label(new Rect(375, 100, 125, 50), "float value " + floatToSave.ToString("F1"));
        GUI.Label(new Rect(375, 200, 125, 50), "bool value " + boolToSave);
        GUI.Label(new Rect(375, 300, 125, 50), "sample text " + stringToSave);

        if (GUI.Button(new Rect(550, 0, 125, 50), "save"))
            SaveGame(datasave);
        if (GUI.Button(new Rect(550, 100, 125, 50), "load"))
            LoadGame(/*datasave*/);
        if (GUI.Button(new Rect(550, 200, 125, 50), "reset"))
            ResetData();
    }

    //Save&Load
    public void SaveGame(object datasave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + save);
        bf.Serialize(file, datasave);
        file.Close();
        Debug.Log("Game data saved!");
        Debug.Log(datasave);
    }

    public void LoadGame(/*object datasave*/)
    {
        if (File.Exists(Application.persistentDataPath + save))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + save, FileMode.Open);
            bf.Deserialize(file);
            file.Close();
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    //public void SaveGame()
    //{
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");
    //    SaveData data = new SaveData();
    //    data.savedInt = intToSave;
    //    data.savedFloat = floatToSave;
    //    data.savedBool = boolToSave;
    //    data.savedString = stringToSave;
    //    bf.Serialize(file, data);
    //    file.Close();
    //    Debug.Log("Game data saved!");
    //}

    //public void LoadGame()
    //{
    //    if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
    //        SaveData data = (SaveData)bf.Deserialize(file);
    //        intToSave = data.savedInt;
    //        floatToSave = data.savedFloat;
    //        boolToSave = data.savedBool;
    //        stringToSave = data.savedString;
    //        file.Close();
    //        Debug.Log("Game data loaded!");

    //    }
    //    else
    //        Debug.LogError("There is no save data!");
    //}

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + save))
        {
            File.Delete(Application.persistentDataPath + save);
            intToSave = 0;
            floatToSave = 0.0f;
            boolToSave = false;
            stringToSave = " ";
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }
}//25