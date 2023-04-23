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

    private Difficulty currentDifficulty;
    private float musicVolumeLevel;
    private float soundVolumeLevel;
    private void Start()
    {
        currentDifficulty = difficultySelection.GetFirstActiveToggle().GetComponent<UIToggleDifficulty>().difficulty;

        UpdateSliders();
    }

    private void UpdateSliders()
    {

        musicVolume.value = PersistenceData.Instance.data.settings.musicVolume;
        soundVolume.value = PersistenceData.Instance.data.settings.soundVolume;

        musicVolumeLevel = musicVolume.value;
        soundVolume.value = soundVolume.value;
    }

    public void OnMusicVolumeChanged()
    {
        musicVolumeLevel = musicVolume.value;

        SoundManager.Instance.SetMusicVolume(musicVolumeLevel);
    }

    public void OnSoundVolumeChanged() => soundVolume.value = soundVolume.value;

    private void SaveSettings()
    {
        PersistenceData.Instance.data.settings.musicVolume = musicVolumeLevel;
        PersistenceData.Instance.data.settings.soundVolume = soundVolumeLevel;

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
