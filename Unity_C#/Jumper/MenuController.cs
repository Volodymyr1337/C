using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pannel;
    public GameObject ScoreTab;
    public GameObject start;
    public Text ControlKeys;
    public Text SoundTxt;
    //score table
    public Text Counter;
    public Text Username;
    public Text Scores;
    public Text Country;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Option()
    {
        pannel.SetActive(true);
        start.SetActive(false);
    }
    public void OptionOK()
    {
        ScoreTab.SetActive(false);
        pannel.SetActive(false);
        start.SetActive(true);
    }
    public void Keys()
    {
        if (PlayerPrefs.GetString("Keys") != "off")
        {
            PlayerPrefs.SetString("Keys", "off");
            ControlKeys.text = "Control mode: acceleration";
        }
        else
        {
            PlayerPrefs.SetString("Keys", "on");
            ControlKeys.text = "Control mode: Control Keys";
        }

    }
    public void Sound()
    {
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            PlayerPrefs.SetString("Sound", "off");
            SoundTxt.text = "Sound: off";
        }
        else
        {
            PlayerPrefs.SetString("Sound", "yes");
            SoundTxt.text = "Sound: on";
        }
    }

    public void ScoreTabOn()
    {
        start.SetActive(false);
        ScoreTab.SetActive(true);
        StartCoroutine(LoadScores());
    }
    // принимаем JSON с сайта
    private IEnumerator LoadScores()
    {
        Counter.text = "";
        Username.text = "";
        Scores.text = "";
        Country.text = "";
        WWW www = new WWW("http://soundlab.com.ua/scrt/GetScores.php");

        yield return www;

        string jsonString = fixJson(www.text);
        Player[] player = JsonHelper.FromJson<Player>(jsonString);
        int len = player.Length;
        if (len > 10)
            len = 10;
        for (var i = 0; i < len; i++)
        {
            Counter.text += (i + 1).ToString() + "\n";
            Username.text += player[i].username + "\n";
            Scores.text += player[i].score + "\n";
            Country.text += player[i].country + "\n";
        }
    }
    // ! фиксим т.к. json ловим с сервера !
    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}

[Serializable]
public class Player
{
    public string id;
    public string username;
    public string score;
    public string country;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

