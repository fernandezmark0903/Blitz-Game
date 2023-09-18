using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public InputField Log_username;
    public InputField Log_password;
    public InputField User_username;
    public InputField User_password;
    public InputField User_name;

    public GameObject popUp;
    public GameObject registerForm;
    public GameObject loginForm;

    public Button registerBtn;

    public Text popupText;
    public Text buttonText;

    public string status;

    public PopUp popupManager = null;


    private void Start()
    {
        popupManager = GetComponent<PopUp>();
    }

    public void enterFullScreen()
    {
        Screen.fullScreen = true;
    }

    public void goBack(int index)
    {
        if (index == 0)
        {
            if (status == "Registration Successfull")
            {
                popUp.SetActive(false);
                loginForm.SetActive(true);
                registerForm.SetActive(false);
            }
            else if (status == "Login Failed" || status == "Incorrect password" || status == "User not found")
            {
                popUp.SetActive(false);
                loginForm.SetActive(true);
                registerForm.SetActive(false);
            }
            else if (status == "Login Successfull")
            {
                //load the scene
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                popUp.SetActive(false);
                loginForm.SetActive(false);
                registerForm.SetActive(true);
            }

        }
        else if (index == 1)
        {
            popUp.SetActive(false);
            registerForm.SetActive(false);
            loginForm.SetActive(true);
        }
        else if (index == 2)
        {
            popUp.SetActive(false);
            registerForm.SetActive(true);
            loginForm.SetActive(false);
        }

    }
}
