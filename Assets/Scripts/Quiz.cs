using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public event EventHandler OnGameEnd;

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;
    [SerializeField] int howManyQuestions;

    [Header("Answer Buttons")]
    [SerializeField] GameObject[] answerButtons;
    private bool hasAnsweredEarly = false;

    [Header("Answer Sprites")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite incorrectAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    private bool isComplete;

    private int correctAnswerIndex = 0;
    private Image buttonImage;
    private List<int> questionIndexes = new List<int>();

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void Start()
    {
        FillQuestionIndexesList();
        Reset();
        TryGetNextQuestion();
        SetUpProgressBar();
        timer.OnAnswerTimerEnd += TryGetNextQuestion;
        timer.OnQuestionTimerEnd += HandleQuestionTimerEnd;
    }

    private void Update()
    {
        if (isComplete) return;

        timerImage.fillAmount = timer.fillFraction;

        if (progressBar.value == howManyQuestions)
        {
            isComplete = true;
            OnGameEnd?.Invoke(this, EventArgs.Empty);
            timer.SwitchTimer();
            return;
        }
    }

    private void HandleQuestionTimerEnd()
    {
        DisplayAnswer(-1);
        SetButtonState(false);
        hasAnsweredEarly = false; // Reset flag for the next question
    }

    private void Reset()
    {
        scoreKeeper.ResetScore();
        isComplete = false;
        hasAnsweredEarly = false;
    }

    private void SetUpProgressBar()
    {
        progressBar.maxValue = howManyQuestions;
        progressBar.value = 0;
    }

    private void FillQuestionIndexesList()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            questionIndexes.Add(i);
        }
        ShuffleList(questionIndexes);
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void DisplayAnswer(int index)
    {
        scoreKeeper.IncrementQuestionsSeen();

        if (index == correctAnswerIndex)
        {
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else if (index == -1)
        {
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        else
        {
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = incorrectAnswerSprite;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void TryGetNextQuestion()
    {
        if (questionIndexes.Count > 0)
        {
            GetNextQuestionIndexFromTheList(questionIndexes[0]);
            questionIndexes.RemoveAt(0);
            SetButtonState(true);
            SetDefaultButtonSprites();
            hasAnsweredEarly = false; // Reset flag for the next question
        }
        else
        {
            questionText.text = "Game Over!";
            isComplete = true;
            OnGameEnd?.Invoke(this, EventArgs.Empty);
            timer.SwitchTimer();
        }
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject button in answerButtons)
        {
            buttonImage = button.GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void GetNextQuestionIndexFromTheList(int currentIndex)
    {
        questionText.text = questions[currentIndex].GetQuestion();
        correctAnswerIndex = questions[currentIndex].GetCorrectAnswerIndex();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questions[currentIndex].GetAnswer(i);
        }
        progressBar.value++;
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.SwitchTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void SetButtonState(bool state)
    {
        foreach (GameObject button in answerButtons)
        {
            button.GetComponent<Button>().interactable = state;
        }
    }
}
