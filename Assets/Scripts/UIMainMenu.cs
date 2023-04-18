using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public TMP_InputField inputFieldName;
    public TextMeshProUGUI textBestScore;

    private void Start()
    {
        DisplayHighScore();
    }

    private void DisplayHighScore()
    {
        Debug.Log("Display high score");
        ScoreData highScore = PersistenceData.Instance.GetHighScore();

        if (highScore != null)
            textBestScore.text = $"Best score <b>{highScore.name} {highScore.score}";
        else
            textBestScore.text = $"No score yet. Be the first!";
    }

    public void OnStartClicked() => SceneManager.LoadScene(1);

    public void OnQuitClicked()
    {
        SaveData();

        if(Application.isEditor)
            EditorApplication.ExitPlaymode();
        else
            Application.Quit();
    }

    public void OnClearData()
    {
        PersistenceData.Instance.ClearData();

        DisplayHighScore();
    }
    private void SaveData() => PersistenceData.Instance.SaveData();

    public void OnNameChanged()
    {
        ScoreData data = new ScoreData();
        data.name = inputFieldName.text;
        data.score = 0;
        
        PersistenceData.Instance.AddScore(data);

        DisplayHighScore();
    }
}
