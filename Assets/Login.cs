using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField email;
    public Text loginSuccessful;
    IEnumerator CreateUser(string username, string password, string email)
    {
        string createUserURL = "http://localhost/nsirpg/InsertUser.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log("webRequest");
    }

    IEnumerator CreateNewLogin(string username, string password)
    {
        string createLoginURL = "http://localhost/nsirpg/Login.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest webRequest = UnityWebRequest.Post(createLoginURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);

        if(webRequest.downloadHandler.text == "Login Successful")
        {
            SceneManager.LoadScene(1);

        }
        else
        {
           
        }
    }

    public void CreateNewUser()
    {
        StartCoroutine(CreateUser(username.text, password.text, email.text ) );
    }

    public void CreateNewLogin()
    {
        StartCoroutine(CreateNewLogin(username.text, password.text));
    }
}
