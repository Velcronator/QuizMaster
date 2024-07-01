using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //serialize field allows you to see the variable in the inspector
    [SerializeField] private float timeToCompleteQuestion = 15f;
    [SerializeField] private float timeToShowCorrectAnswer = 10f;

    //todo: Event Publisher
    public bool isAnsweringQuestion = false;

    float timerValue = 10;
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        //Debug.Log(timerValue);

        if (isAnsweringQuestion)
        {
            if (timerValue <= 0)
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
                //Debug.Log("Time is up!");
            }
        }
        else
        {
            if (timerValue <= 0)
            {
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                //Debug.Log("Time to answer the question!");
            }
        }
    }
}
