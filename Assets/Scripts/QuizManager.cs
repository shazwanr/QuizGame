using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuizManager : MonoBehaviour
{
    public List<QnA> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public Text QuestionText;
    public Text ScoreText;
    public Text QuestionNum;
    public int score;
    int totalQuestions;
    int count = 1;

    public GameObject QuizPanel;
    public GameObject GameOverPanel;
    public GameObject PausePanel;

    public GameObject Playbutton;
    public GameObject StopButton;

    AudioSource sound;

    ColorBlock colorblock;

    private void Start()
    {
        totalQuestions = QnA.Count;
        GameOverPanel.SetActive(false);
        StopButton.SetActive(false);
        PausePanel.SetActive(false);
        generateQuestion();
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        ScoreText.text = score + "/" + totalQuestions;
    }

    public void Pause()
    {
        StopSound();
        PausePanel.SetActive(true);
    }

    public void Unpause()
    {
        PausePanel.SetActive(false);
    }

    public void PlaySound()
    {
        Playbutton.SetActive(false);
        StopButton.SetActive(true);
        sound.Play();
    }

    public void StopSound()
    {
        StopButton.SetActive(false);
        Playbutton.SetActive(true);
        sound.Stop();
    }


    public void Correct()
    {
        StopSound();
        score += 1;
        StartCoroutine(Delay());
    }

    public void Wrong()
    {
        StopSound();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        disablebuttons();
        yield return new WaitForSeconds(1.5f);
        QnA.RemoveAt(currentQuestion);
        EventSystem.current.SetSelectedGameObject(null);
        count++;
        enablebuttons();
        generateQuestion();
    }

    public void enablebuttons()
    {
        Playbutton.GetComponent<Button>().enabled = true;
        StopButton.GetComponent<Button>().enabled = true;
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Button>().interactable = true;
        }
    }

    public void disablebuttons()
    {
        Playbutton.GetComponent<Button>().enabled = false;
        StopButton.GetComponent<Button>().enabled = false;
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Button>().interactable = false;
        }
    }

    void SetAnswers()
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            colorblock = options[i].GetComponent<Button>().colors;
            colorblock.disabledColor = Color.red;
            options[i].GetComponent<Button>().colors = colorblock;

            if (QnA[currentQuestion].CorrectAns == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
                colorblock.disabledColor = Color.green;
                options[i].GetComponent<Button>().colors = colorblock;
            }
        }
    }


    void generateQuestion()
    {
        QuestionNum.text = "Question " + count;
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionText.text = QnA[currentQuestion].Question;
            sound = QnA[currentQuestion].audioSource;
            SetAnswers();
        }
        else
        {
            Debug.Log("Finished");
            GameOver();
        }
    }


}
