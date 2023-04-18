using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersistenceData : MonoBehaviour
{
    public static PersistenceData Instance;

    public Data data;
    string path;
    string PlayerName;

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

        PlayerName = "";
    }

    public void AddScore(ScoreData data)
    {
        this.data.AddData(data);

        SaveData();
    }
    public void SetPlayerName(string text) => this.PlayerName = text;
    public string GetPlayerName() => PlayerName;
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

    internal void SaveScore(ScoreData data)
    {
        this.data.AddPlayerScore(data);

        SaveData();
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

    public void AddPlayerScore(ScoreData score)
    {
        Debug.Log($"Adding new score {score}");
        if( data.Any(element => element.name == score.name))
        {
            Debug.Log($"Data have player with name {score.name} and old score {score.score}");
            ScoreData element = data.FirstOrDefault(el => el.name == score.name);
            if (element.score > score.score)
                return;

            element.score = score.score;
        }
        else
            data.Add(score);
    }
}
