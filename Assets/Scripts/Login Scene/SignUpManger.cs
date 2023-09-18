using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SignUpManger : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void insertUser(string path, string objectName, string username, string password, string name, int score1, int score2, int score3, string callback);

    ObjectManager objManager = null;
 

    public void insertData()
    {
        ObjectManager objManager = this.GetComponent<ObjectManager>();

        if (objManager.User_username.text == "" || objManager.User_password.text == "" || objManager.User_name.text == "")
        {
            objManager.registerForm.SetActive(false);
            objManager.popUp.SetActive(true);
            objManager.status = "Fill up all the fields";
            objManager.popupText.text = objManager.status;
            objManager.popupText.color = Color.red;
        }
        else
        {
            string username = objManager.User_username.text.ToLower();
            string password = objManager.User_password.text.ToLower();
            string name = objManager.User_name.text.ToLower();
            insertUser(path: "users", gameObject.name,username: username, password: password, name: name, score1: 0, score2: 0, score3: 0, callback: "showPopUp");

        }
    }

    private void showPopUp(string data)
    {
        objManager.popupManager.showPopUp(data);
    }

}
