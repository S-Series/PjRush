using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDB : MonoBehaviour
{
    public static PlayerDB playerDB;

    [SerializeField]
    public Player playerData = new Player();

    private void Awake()
    {
        playerDB = this;
    }

    void Start()
    {
        try
        {
            Load();
        }
        catch { }
    }

    void Update() { if (Input.GetKeyDown(KeyCode.Space)) Save(); }

    [ContextMenu("Save")]
    public void SaveDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "PlayerDB.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load")]
    void LoadDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "PlayerDB.json");
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<Player>(jsonData);
    }

    public void Save()
    {
       
    }

    public void Load()
    {

    }
}

[System.Serializable]
public class Player
{
    public int gold;
    public int level;
    public int experience;
}
