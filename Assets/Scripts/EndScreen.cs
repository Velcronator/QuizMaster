using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if (scoreKeeper == null)
        {
            Debug.LogError("ScoreKeeper not found in the scene!");
        }
    }

    public void ShowFinalScore()
    {
        if (finalScoreText == null)
        {
            Debug.LogError("FinalScoreText is not assigned!");
            return;
        }

        int tempScore = scoreKeeper.CalculateScore();
        finalScoreText.text = $"Final Score: \nYou got a score of {tempScore:F0}%";
    }
}
