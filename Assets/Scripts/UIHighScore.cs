using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHighScore : MonoBehaviour
{
    public GameObject itemsParent;

    UIHighScoreItem[] items;

    private void Awake()
    {
    }
    void Start()
    {
        items = itemsParent.GetComponentsInChildren<UIHighScoreItem>();

        SetupHighScoreUI();
    }

    private void SetupHighScoreUI()
    {
        PersistenceData.Instance.data.Sort();

        for (int i = 0; i < PersistenceData.Instance.data.Count; i++)
        {
            items[i].UpdateData(i, PersistenceData.Instance.data.data[i]);
        }
    }

    public void OnMainMenuClicked() => SceneManager.LoadScene(0);

}
