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
    public InputField updatedPassword;
    public Text loginSuccessful;
   // public int[] number = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
   // public string[] letters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
   // public int randomNum;
   // public int randomLet;
    private string user;

    private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string code = "";

    void CreateCode()
    {
        for (int i = 0; i < 4; i++)
        {
            int a = UnityEngine.Random.Range(0, characters.Length);
            code = code + characters[a];
        }

        Debug.Log(code);
    }
    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        string createUserURL = "http://localhost/nsirpg/InsertUser.php";
        WWWForm form = new WWWForm();
        form.AddField("username", _username);
        form.AddField("password", _password);
        form.AddField("email", _email);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
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
        CreateCode();
        Debug.Log(email.text);
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("sqlunityclasssydney@gmail.com");
        mail.To.Add(email.text);
        mail.Subject = "NSIRPG Password Reset";
        mail.Body = "Hello " + user + "\nReset using this code: " + code;

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
        //HERE
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
    IEnumerator UpdatePassword(InputField username, InputField password)
    {
        string createUserURL = "http://localhost/nsirpg/UpdatePassword.php";
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", password.text);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if (webRequest.downloadHandler.text == "Password Changed")
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
        else
        {
            user = webRequest.downloadHandler.text;
        
        }
    }
    public void UpdatedPassword()
    {
        StartCoroutine(UpdatePassword(username, password));
    }
    public void LoginExistingUser()
    {
        StartCoroutine(CreateNewLogin(username.text, password.text));
    }
    public void CheckEmail(InputField email)
    {
        StartCoroutine(ForgetUser(email));
    }
    //function that turns on a panel that contains an input field and a button  
    //this function runs at the end of SendEmail where the HERE text is

    //when we input the code and hit the button...the button runs a new function
    //Function checks if inputfield.text == code if yes it changes Panel to the Password Update Panel

    //when both new and check password fields are the same and not empty...submit button is on else its off
    //Submit button has a function that is on the button
    //public void updatepassword

    //this runs the IEnumerator password update
    //this requires password.text from a field and the user 

}
