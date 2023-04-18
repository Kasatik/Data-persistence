using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersistenceData : MonoBehaviour
{
    public static PersistenceData Instance;

    public Data data;
    string path;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        path = Application.persistentDataPath + "/scoredata.json";

        LoadData();
    }

    public void AddScore(ScoreData data)
    {
        this.data.AddData(data);

        SaveData();
    }

    public ScoreData GetHighScore()
    {
        if (data.Count == 0)
            return null;

        data.Sort();

        return data.GetHighScore();
    }
    public void LoadData()
    {
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log($"Load data {json}");
            data = JsonUtility.FromJson<Data>(json);

            return;
        }

        data = new Data();
    }
    public void SaveData()
    {
        Debug.Log(data.ToString());
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(path, json);

        Debug.Log($"Save data {json}");
    }

    public void ClearData()
    {
        File.Delete(path);

        LoadData();
    }
}
    
[Serializable]
public class ScoreData : IComparable<ScoreData>
{
    public string name;
    public int score;

    public int CompareTo(ScoreData other)
    {
        return other.score.CompareTo(this.score);
    }

    public override string ToString()
    {
        return name + " " + score;
    }
}

[Serializable]
public class Data
{
    public List<ScoreData> data;

    public Data() => this.data = new List<ScoreData>();

    public int Count { get { return data.Count; } }
    internal void AddData(ScoreData data) => this.data.Add(data);

    public void Sort() => this.data.Sort();

    public ScoreData GetHighScore() => this.data[0];
}
