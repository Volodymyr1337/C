using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataInserter : MonoBehaviour {

    public InputField input;
	private string inputUserName;
	private string inputScore;
    
    private string CreateUserURL;

    public IEnumerator Load()
    {
        inputScore = PlayerController.Instance.scores.ToString();
        inputUserName = input.text;
        var chars = inputUserName.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
        {
            if (!System.Char.IsLetter(chars[i]))
            {
                chars[i] = '_';
            }
        }
        inputUserName = new string(chars);
        CreateUserURL = "http://soundlab.com.ua/scrt/InsertUser.php?usernamePost=" + inputUserName + "&" + "scorePost=" + inputScore;
        WWW www = new WWW(CreateUserURL);

        yield return www;
    }

    public void InputValue()
    {
        if (input.IsActive())
        {
            StartCoroutine(Load());
        }
        
        Debug.Log("you enter" + inputUserName);
        SceneManager.LoadScene(0);
    }
}
