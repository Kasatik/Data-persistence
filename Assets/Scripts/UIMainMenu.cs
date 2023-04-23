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
        DisplayPlayerName();
        SetMusicVolume();
    }

    private void SetMusicVolume()
    {
        SoundManager.Instance.SetMusicVolume(PersistenceData.Instance.data.settings.musicVolume);
    }

    private void DisplayPlayerName() => inputFieldName.text = PersistenceData.Instance.GetPlayerName();

    private void DisplayHighScore()
    {
        Debug.Log("Display high score");
        ScoreData highScore = PersistenceData.Instance.GetHighScore();

        if (highScore != null)
            textBestScore.text = $"Best score <b>{highScore.name}: {highScore.score}";
        else
            textBestScore.text = $"No score yet. Enter your name to start!";
    }

    public void OnStartClicked()
    {
        if (string.IsNullOrEmpty(PersistenceData.Instance.GetPlayerName()))
            return;

        SceneManager.LoadScene(1);
    }

    public void OnHighScoreClicked() => SceneManager.LoadScene(2);

    public void OnSettingClicked() => SceneManager.LoadScene(3);    
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
        inputFieldName.text = "";
        DisplayHighScore();
    }
    private void SaveData() => PersistenceData.Instance.SaveData();

    public void OnNameChanged()
    {
        PersistenceData.Instance.SetPlayerName(inputFieldName.text);

        DisplayHighScore();
    }
}
