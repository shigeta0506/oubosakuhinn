using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 500;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score -= points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (score >= 0)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        if(score == 0)
        {
            SpawnMino.isClear = true;
        }
    }
}
