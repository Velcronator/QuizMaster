using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnQuestionTimerEnd;
    public event Action OnAnswerTimerEnd;

    [SerializeField] private float timeToCompleteQuestion = 30f;
    [SerializeField] private float timeToShowCorrectAnswer = 5f;


    // fillfraction getter private setter
    public float fillFraction { get; private set; }

    private Quiz quiz;
    private bool isGameOver;
    bool isQuestionStateTimer = true;

    private float timerValue;

    private void Awake()
    {
        quiz = FindObjectOfType<Quiz>();
        if (quiz == null)
        {
            Debug.LogError("Quiz not found in the scene!");
        }
    }

    private void Start()
    {
        timerValue = timeToCompleteQuestion;
        isQuestionStateTimer = true;

        if (quiz != null)
        {
            quiz.OnGameEnd += HandleOnGameEnd;
        }
    }

    private void OnDestroy()
    {
        if (quiz != null)
        {
            quiz.OnGameEnd -= HandleOnGameEnd;
        }
    }

    private void HandleOnGameEnd(object sender, EventArgs e)
    {
        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver) return;
        UpdateTimer();
    }

    public void SwitchTimer()
    {
        // Switch the timer state
        if (isQuestionStateTimer)
        {
            isQuestionStateTimer = false;
            timerValue = timeToShowCorrectAnswer;
        }
        else
        {
            isQuestionStateTimer = true;
            timerValue = timeToCompleteQuestion;
        }
    }

    private void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isQuestionStateTimer)
        {   // Answering the question
            if (timerValue > 0)
            {   // Still time to answer
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else
            {   // Time is up
                isQuestionStateTimer = false;
                timerValue = timeToShowCorrectAnswer;
                OnQuestionTimerEnd?.Invoke();
            }
        }
        else
        {   // Showing the correct answer
            if (timerValue > 0)
            {   // Still time to show the correct answer
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {   // Time is up for the correct answer
                isQuestionStateTimer = true;
                timerValue = timeToCompleteQuestion;
                OnAnswerTimerEnd?.Invoke();
            }
        }
    }
}
