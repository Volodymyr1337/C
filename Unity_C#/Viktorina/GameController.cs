using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public QuestionList[] questions;

    public Text QText;                          
    public Text[] answText;                     // ответы
    public GameObject headPannel;
    public Button[] answBtn = new Button[3];    
    public Sprite[] Icons = new Sprite[2];
    public Image Icon;
    public Text WLtxt;

    private int rand;
    QuestionList currQuestion;
    List<object> qList;                         // вопросы

    public void OnStart()
    {
        qList = new List<object>(questions);    //записываем вопросы в список
        QuestionGenerate();
        if (!headPannel.GetComponent<Animator>().enabled)
            headPannel.GetComponent<Animator>().enabled = true;
        else
            headPannel.GetComponent<Animator>().SetTrigger("In");
    }
    private void QuestionGenerate()
    {
        QText.gameObject.GetComponent<Animator>().SetTrigger("In");

        if (qList.Count > 0)
        {
            rand = UnityEngine.Random.Range(0, qList.Count);    // рандомим вопрос
            currQuestion = qList[rand] as QuestionList;
            QText.text = currQuestion.question;

            List<string> answers = new List<string>(currQuestion.answers);

            for (var i = 0; i < currQuestion.answers.Length; i++)
            {
                int randAnsw = UnityEngine.Random.Range(0, answers.Count);
                answText[i].text = answers[randAnsw];
                answers.RemoveAt(randAnsw);
            }
            StartCoroutine(AnimButtons());
        }
        else
        {
            Debug.Log("Вы прошли игру!");
        }
    }
    public void AnswerButton(int index)
    {
        if (answText[index].text.ToString() == currQuestion.answers[0])
            StartCoroutine(TrueOrFalse(true));
        else
            StartCoroutine(TrueOrFalse(false));
        
    }
    IEnumerator AnimButtons()
    {
        yield return new WaitForSeconds(1);
        for (var i = 0; i < answBtn.Length; i++)
        {
            answBtn[i].interactable = false;
        }
        int n = 0;
        while (n < answBtn.Length)
        {
            if (!answBtn[n].gameObject.activeSelf)
                answBtn[n].gameObject.SetActive(true);
            else
                answBtn[n].gameObject.GetComponent<Animator>().SetTrigger("In");
            n++;
            yield return new WaitForSeconds(.5f);
        }
        for (var i = 0; i < answBtn.Length; i++)
        {
            answBtn[i].interactable = true;
        }
        yield break;
    }
    
    IEnumerator TrueOrFalse(bool check)         // ответ верный или нет
    {
        for (var i = 0; i < answBtn.Length; i++)
            answBtn[i].interactable = false;

        yield return new WaitForSeconds(.5f);
        for (var i = 0; i < answBtn.Length; i++)
        {
            answBtn[i].gameObject.GetComponent<Animator>().SetTrigger("Out");
        }

        QText.gameObject.GetComponent<Animator>().SetTrigger("Out");

        yield return new WaitForSeconds(.5f);

        if (!Icon.gameObject.activeSelf)
            Icon.gameObject.SetActive(true);

        if (check)
        {
            Icon.sprite = Icons[0];
            Icon.gameObject.GetComponent<Animator>().SetTrigger("In");
            WLtxt.text = "Ответ верный!";

            yield return new WaitForSeconds(1f);

            Icon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            qList.RemoveAt(rand);
            yield return new WaitForSeconds(.5f);
            QuestionGenerate();
            yield break;
        }
        else
        {
            Icon.sprite = Icons[1];
            WLtxt.text = "Ответ неверный!";
            Icon.gameObject.GetComponent<Animator>().SetTrigger("In");
            yield return new WaitForSeconds(1f);

            Icon.gameObject.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(.5f);
            headPannel.GetComponent<Animator>().SetTrigger("Out");
            yield break;
        }
    }
}
[Serializable]
public class QuestionList
{
    public string question;
    public string[] answers = new string[3];
}