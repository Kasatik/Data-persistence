using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;

    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private float difficulty = 0f;
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);

                brick.onDestroyed.AddListener(PlaySound);
            }
        }

        DisplayHighScore();
        DisplayPlayerName();
        SetVolume();
        
        SetDifficulty();
    }

    private void SetDifficulty()
    {
        switch(PersistenceData.Instance.GetDifficultySetting())
        {
            case Difficulty.Easy:
                difficulty = 1f;
                break;
            case Difficulty.Medium:
                difficulty = 1.6f;
                break;
            case Difficulty.Hard:
                difficulty = 2.3f;
                break;
            default:
                difficulty = 1f;
                break;
        }
    }

    private void SetVolume()
    {
        SoundManager.Instance.SetMusicVolume(PersistenceData.Instance.data.settings.musicVolume);
        SoundManager.Instance.SetSoundEffectVolume(PersistenceData.Instance.data.settings.soundVolume);
    }

    private void PlaySound(int arg0) => SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.soundEffect);

    private void DisplayPlayerName()
    {
        ScoreText.text = $"Score {PersistenceData.Instance.GetPlayerName()}: {m_Points}";
    }

    private void DisplayHighScore()
    {
        ScoreData highScore = PersistenceData.Instance.GetHighScore();
        
        bestScoreText.text = $"Best score <b>{highScore.name} {highScore.score}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f * difficulty, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score {PersistenceData.Instance.GetPlayerName()}: {m_Points}";
    }

    public void GameOver()
    {

        ScoreData data = new ScoreData();
        data.name = PersistenceData.Instance.GetPlayerName();
        data.score = m_Points;

        PersistenceData.Instance.SaveScore(data);

        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
