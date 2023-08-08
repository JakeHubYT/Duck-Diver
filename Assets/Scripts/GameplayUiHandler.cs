using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUiHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public CollectItem collectItem;

    private int score = 0;

    private void Start()
    {
        UpdateScoreText();

        collectItem.onCollectBreadEvent.AddListener(AddToScore);
    }

    public void AddToScore()
    {
        score++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

}
