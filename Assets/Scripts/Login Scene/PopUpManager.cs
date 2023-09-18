using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    ObjectManager objManager = null;
    public void showPopUp(string data)
    {
        ObjectManager objManager = this.GetComponent<ObjectManager>();

        objManager.loginForm.SetActive(false);
        objManager.registerForm.SetActive(false);
        objManager.popUp.SetActive(true);
        objManager.status = data;

        if (data == "Registration Successfull" || data == "Login Successfull")
        {
            objManager.popupText.text = data;
            objManager.popupText.color = Color.green;
            objManager.User_username.text = "";
            objManager.User_password.text = "";
            objManager.User_name.text = "";
            objManager.Log_username.text = "";
            objManager.Log_password.text = "";
            objManager.buttonText.text = "OKAY";

        }
        else
        {
            objManager.popupText.text = data;
            objManager.popupText.color = Color.red;
            objManager.buttonText.text = "BACK";
        }

    }
}
