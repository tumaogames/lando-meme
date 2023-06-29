using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class DataManager : MonoBehaviour
{
    public GameManager gameManager;
    private string file = "images.txt";
    string json;
    int index = 0;
    public string path;
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

    private void Awake()
    {
        path = GetFilePath(file);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        path = GetFilePath(file);
        if (File.Exists(path))
        {
            Load();
        }
        else
        {
            Save();
        }
    }

    void Start()
    {

    }

    public void Save()
    {
        json = "";
        for (int i = 0; i < gameManager.images.Count; i++)
        {
            gameManager.images[i].memeSprite = null;
            json += JsonUtility.ToJson(gameManager.images[i]) + "\r\n";
        }
        WriteToFile(file, json);
        Load();
    }

    public void Load()
    {
        path = GetFilePath(file);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                while ((path = reader.ReadLine()) != null)
                {
                    gameManager.images[index] = MemeImage.CreateFromJSON(path);
                    gameManager.images[index].memeSprite = Resources.Load<Sprite>(gameManager.images[index].name); ;
                    index++;
                }
                index = 0;
            }
        }

    }

    public void WriteToFile(string fileName, string json)
    {
        path = GetFilePath(file);

        FileStream filestream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(filestream))
        {
            writer.Write(json);
        }
    }



    public string GetFilePath(string fileName)
    {
        string mystring = Application.persistentDataPath + "/" + fileName;
        return mystring;
    }

    public void ClearSaveButton()
    {
        foreach (InputField inputField in GameObject.FindObjectsOfType<InputField>())
        {
            inputField.Select();
            inputField.text = "";
            /*foreach (Text text in inputField.GetComponentsInChildren<Text>())
            {
                if (text.gameObject.name != "Placeholder")
                {
                    text.Select();
                }
                
            }*/
        }
    }

    public void LikeUrl()
    {
        Application.OpenURL("https://facebook.com/tumaogames/");
    }
}
