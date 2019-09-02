using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region THIS SHIT FOR SENDING EMAILS
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
#endregion

public class Login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField email;
    public Text loginSuccessful;
    public int[] number = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public string[] letters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
    public int randomNum;
    public int randomLet;
    private string user;


    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        string createUserURL = "http://localhost/nsirpg/InsertUser.php";
        WWWForm form = new WWWForm();
        form.AddField("username", _username);
        form.AddField("password", _password);
        form.AddField("email", _email);
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

   void SendEmail(InputField email)
    {
        RandomCodeCGenerator();
        Debug.Log(email.text);
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("sqlunityclasssydney@gmail.com");
        mail.To.Add(email.text);
        mail.Subject = "NSIRPG Password Reset";
        mail.Body = "Hello " + user + "\nReset using this code:" + randomLet;

        // connect to google
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        // the 
        smtpServer.Port = 25; //88 25
        // Login to google
        smtpServer.Credentials = new NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };
        //Send message
        smtpServer.Send(mail);
        Debug.Log("Sending Email....");
    }


    void RandomCodeCGenerator()
    {
        randomLet = (UnityEngine.Random.Range(0, letters.Length));
    }

    IEnumerator ForgetUser(InputField email)
    {
        string createUserURL = "http://localhost/nsirpg/checkemail.php";
        WWWForm form = new WWWForm();
        form.AddField("email_Post", email.text);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL, form);
        yield return webRequest.SendWebRequest(); 
        Debug.Log(webRequest.downloadHandler.text);
        if (webRequest.downloadHandler.text == "User Not Found")
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            user = webRequest.downloadHandler.text;
            SendEmail(email); 

        }
    }  

    public void LoginExistingUser()
    {
        StartCoroutine(CreateNewLogin(username.text, password.text));
    }

    public void CheckEmail(InputField email)
    {
        StartCoroutine(ForgetUser(email));
    }
}
