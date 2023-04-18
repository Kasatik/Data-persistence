using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHighScoreItem : MonoBehaviour
{
    public TextMeshProUGUI orderNumber;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI score;

    private void Awake()
    {
        orderNumber.text = "N/A";
        playerName.text = "no name";
        score.text = "0";
    }

    public void UpdateData(int index, ScoreData data)
    {
        orderNumber.text = index.ToString();
        playerName.text = data.name;
        score.text = data.score.ToString();
    }
}
