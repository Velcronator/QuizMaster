using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite incorrectAnswerSprite;

    private int currentAnswerIndex = 0;
    Image buttonImage;

    void Start()
    {
        GetNextQuestion();
    }

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
    }

    private void SetDefaultButtonSprites()
    {   
        foreach (GameObject button in answerButtons)
        {
            buttonImage = button.GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Length);
        questionText.text = questions[randomIndex].GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questions[randomIndex].GetAnswer(i);
            // get the correct answer index
            currentAnswerIndex = questions[randomIndex].GetCorrectAnswerIndex();
        }
    }

    // On answer selected
    public void OnAnswerSelected(int index)
    {
        SetButtonState(false);
        if (index == currentAnswerIndex)
        {
            Debug.Log("Correct Answer");
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        else
        {
            Debug.Log("Wrong Answer");
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = incorrectAnswerSprite;
        }
        Invoke("GetNextQuestion", 2f);
    }

    void SetButtonState(bool state)
    {
        foreach (GameObject button in answerButtons)
        {
            button.GetComponent<Button>().interactable = state;
        }
    }
}
