using System.Text;
using System.IO;                                              // File Mode: İşletim sisteminin dosyayı nasıl açacağı
using System.Collections;                                     // Create: new file oluşturur. file varsa overwrites
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        Debug.Log("saving to:" + path);

        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            Transform playerTransform = GetplayerTransform();

            BinaryFormatter formatter= new BinaryFormatter();
            SerializableVector3 position = new SerializableVector3(playerTransform.position);
            formatter.Serialize(stream, position);

            //byte[] buffer = SerializeVector(playerTransform.position);
            //stream.Write(buffer, 0, buffer.Length);
        }
    }

    public void Load(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        Debug.Log("loading from:" + GetPathFromSaveFile(saveFile));

        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            //byte[] buffer = new byte[stream.Length];
            //stream.Read(buffer, 0, buffer.Length);

            Transform playerTransform = GetplayerTransform();

            BinaryFormatter formatter = new BinaryFormatter();
            SerializableVector3 position = (SerializableVector3)formatter.Deserialize(stream);
            playerTransform.position = position.ToVector();

           // playerTransform.position = DeserializeVector(buffer);

            //Encoding.UTF8.GetString(buffer);
        }

    }
    private Transform GetplayerTransform()
    {
        return GameObject.FindWithTag("Player").transform;
    }
    private string GetPathFromSaveFile(string saveFile) // save file'ın yolunu kaydederiz
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }

    /*
    private byte[] SerializeVector(Vector3 vector)
    {
        byte[] vectorBytes = new byte[3 * 4]; //bi float 4 byte olduğu için
        BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
        BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
        BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
        return vectorBytes;
    }
    private Vector3 DeserializeVector(byte[] buffer)
    {
        Vector3 vector = new Vector3();
        vector.x = BitConverter.ToSingle(buffer, 0); //ToSingle (byte[], int32) okur. 4 byte'a kadar okur kısaca.
        vector.y = BitConverter.ToSingle(buffer, 4);
        vector.z = BitConverter.ToSingle(buffer, 8);
        return vector;
    }
    */
}

[System.Serializable]
public class SerializableVector3
{
    float x, y, z;
    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3 ToVector()
    {
        return new Vector3(x, y, z);
    }
}
