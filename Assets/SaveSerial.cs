using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class SaveSerial : MonoBehaviour
{
    string stringToSave;
    int intToSave;
    float floatToSave;
    bool boolToSave;

    //GUI 
    void OnGUI() 
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
            SaveGame();
        if (GUI.Button(new Rect(550, 100, 125, 50), "load"))
            LoadGame();
        if (GUI.Button(new Rect(550, 200, 125, 50), "reset"))
            ResetData();
    }

    [System.Serializable]
    class SaveData
    {
        public string savedString;
        public int savedInt;
        public float savedFloat;
        public bool savedBool;
    }

    //Serialization of game objects 
    //нужно привязать к сейву
    [System.Serializable]
    public struct SurrogateQuaternion
    {

        public float x, y, z, w;

        public SurrogateQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }

        public static implicit operator Quaternion(SurrogateQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public static implicit operator SurrogateQuaternion(Quaternion rValue)
        {
            return new SurrogateQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }

    [System.Serializable]
    public struct SurrogateVector3
    {

        public float x, y, z;

        public SurrogateVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        public static implicit operator Vector3(SurrogateVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        public static implicit operator SurrogateVector3(Vector3 rValue)
        {
            return new SurrogateVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    public class BaseData
    {

        [System.NonSerialized] private GameObject _inst; // Link to the object itself
        public GameObject Inst { set { _inst = value; } }
        public string Name { get; set; }
        public SurrogateVector3 Position { get; set; }
        public SurrogateQuaternion Rotation { get; set; }

        public BaseData() { }

        public BaseData(string name, Vector3 position, Quaternion rotation)
        {
            this.Name = name;
            this.Position = position;
            this.Rotation = rotation;
        }

        public BaseData(GameObject current, string name, Vector3 position, Quaternion rotation)
        {
            this.Inst = current;
            this.Name = name;
            this.Position = position;
            this.Rotation = rotation;
        }

        public virtual void Update()
        {
            if (_inst == null) // If the object has been deleted, it will not be restored after loading the scene
            {
                this.Name = null;
                return;
            }

            Position = _inst.transform.position;
            Rotation = _inst.transform.rotation;
        }
    }

    [System.Serializable]
    public class SceneState
    {

        public List<BaseData> itemList = new List<BaseData>(); // List all objects fo serialization

        public SceneState() { }

        public void AddItem(BaseData item)
        {
            itemList.Add(item);
        }

        public void Update()
        {
            foreach (BaseData t in itemList)
                t.Update();
        }
    }

    public class Serializator
    {

        public static void SaveObject(SceneState state, string dataPath)
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Create);
            binary.Serialize(stream, state);
            stream.Close();
            Debug.Log("[Serializator] --> Сохранение по адресу: " + dataPath);
        }

        public static SceneState LoadObject(string dataPath)
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);
            SceneState state = (SceneState)binary.Deserialize(stream);
            stream.Close();
            Debug.Log("[Serializator] --> Загрузка данных из файла: " + dataPath);
            return state;
        }
    }

    //Save&Load
    void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");
        SaveData data = new SaveData();
        data.savedInt = intToSave;
        data.savedFloat = floatToSave;
        data.savedBool = boolToSave;
        data.savedString = stringToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            intToSave = data.savedInt;
            floatToSave = data.savedFloat;
            boolToSave = data.savedBool;
            stringToSave = data.savedString;
            file.Close();
            Debug.Log("Game data loaded!");

        }
        else
            Debug.LogError("There is no save data!");
    }

    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveData.dat");
            intToSave = 0;
            floatToSave = 0.0f;
            boolToSave = false;
            stringToSave = " ";
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }//Не является методом, нужен, чтобы чистить файл

    // Encryption 
    public static class AES
    {
        public static int KeyLength = 128;
        private const string SaltKey = "ShMG8hLyZ7k~Ge5@";
        private const string VIKey = "~6YUi0Sv5@|{aOZO"; // TODO: Generate random VI each encryption and store it with encrypted value

        public static string Encrypt(byte[] value, string password)
        {
            var keyBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(SaltKey)).GetBytes(KeyLength / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.UTF8.GetBytes(VIKey));

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(value, 0, value.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();
                    memoryStream.Close();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Encrypt(string value, string password)
        {
            return Encrypt(Encoding.UTF8.GetBytes(value), password);
        }

        public static string Decrypt(string value, string password)
        {
            var cipherTextBytes = Convert.FromBase64String(value);
            var keyBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(SaltKey)).GetBytes(KeyLength / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.UTF8.GetBytes(VIKey));

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                    memoryStream.Close();
                    cryptoStream.Close();

                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                }
            }
        }
    }//связь с сейвом?
}