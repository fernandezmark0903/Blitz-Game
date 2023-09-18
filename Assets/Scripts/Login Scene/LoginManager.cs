using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    ObjectManager objManager = null;

    [DllImport("__Internal")]
    private static extern void loginUser(string path, string objectName, string username, string password, string callback,string getData);
 

    public void Login()
    {
        objManager = this.GetComponent<ObjectManager>();

        if (objManager.Log_username.text == "" || objManager.Log_password.text == "")
        {
            objManager.loginForm.SetActive(false);
            objManager.registerForm.SetActive(false);
            objManager.popUp.SetActive(true);
            objManager.status = "Login Failed";
            objManager.popupText.text = objManager.status;
            objManager.popupText.color = Color.red;
        }
        else
        {
            string username = objManager.Log_username.text.ToLower();
            string password = objManager.Log_password.text.ToLower();
            loginUser(path: "users", gameObject.name, username: username, password: password, callback: "showPopUp",getData: "getUserData");
        }
    }

    private void getUserData(string data)
    {
        PlayerPrefs.SetString("playerName", data);
    }

    public void loginAsGuest()
    {
        PlayerPrefs.SetString("playerName", "");
        SceneManager.LoadScene("SampleScene");
    }

    private void showPopUp(string data)
    {
        objManager.popupManager.showPopUp(data);
    }
}
