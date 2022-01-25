using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Text;



    public class ClassField : MonoBehaviour
    {

    [System.Serializable]

        public class SaveData
        {
            string savedString;
            int savedInt;
            float savedFloat;
            bool savedBool;
        }

    }
