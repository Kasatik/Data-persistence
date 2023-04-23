using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UISettings : MonoBehaviour
{
    public Slider musicVolume;
    public Slider soundVolume;
    public ToggleGroup difficultySelection;

    private float musicVolumeLevel;
    private float soundVolumeLevel;

    private Dictionary<Difficulty, Toggle> togglesDictionary;

    private void Awake()
    {
        togglesDictionary = new Dictionary<Difficulty, Toggle>();
        Toggle[] toggles = GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            togglesDictionary.Add(toggles[i].GetComponent<UIToggleDifficulty>().difficulty, toggles[i]);
        }
    }
    private void Start()
    {
        UpdateSliders();
        UpdateToggleGroup();
    }

    private void UpdateToggleGroup()
    {
        Difficulty difficulty = PersistenceData.Instance.data.settings.difficulty;

        foreach(KeyValuePair<Difficulty, Toggle> entry in togglesDictionary)
        {
            if (entry.Key == difficulty) 
                entry.Value.isOn = true;
            else
                entry.Value.isOn = false;
        }
    }

    private void UpdateSliders()
    {
        musicVolume.value = PersistenceData.Instance.data.settings.musicVolume;
        soundVolume.value = PersistenceData.Instance.data.settings.soundVolume;

        musicVolumeLevel = musicVolume.value;
        soundVolumeLevel = soundVolume.value;
    }

    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolume.value;

        SoundManager.Instance.SetMusicVolume(musicVolumeLevel);
    }

    public void OnSoundVolumeChanged() => soundVolumeLevel = soundVolume.value;

    private void SaveSettings()
    {
        PersistenceData.Instance.data.settings.musicVolume = musicVolumeLevel;
        PersistenceData.Instance.data.settings.soundVolume = soundVolumeLevel;

        PersistenceData.Instance.data.settings.difficulty = difficultySelection.GetFirstActiveToggle().
                                                                                GetComponent<UIToggleDifficulty>().difficulty;

        PersistenceData.Instance.SaveSettings();
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene(0);
        SaveSettings();
    }
}

public enum Difficulty
{
    Easy, Medium, Hard
}
