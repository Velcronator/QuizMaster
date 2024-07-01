using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;
    [SerializeField] GameObject[] answerButtons;

    void Start()
    {
        GetRandomQuestion();
    }

    void GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Length);
        questionText.text = questions[randomIndex].GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Debug.Log(questions[randomIndex].GetAnswer(i));
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questions[randomIndex].GetAnswer(i);
        }
    }

    // how to get a specific question
    void GetSpecificQuestion(int index)
    {
        questionText.text = questions[index].GetQuestion();
    }






}
