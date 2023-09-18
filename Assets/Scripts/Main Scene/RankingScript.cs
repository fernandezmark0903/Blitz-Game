using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RankingScript : MonoBehaviour
{


    [System.Serializable]
    public class UserData
    {
        public string name;
        public string password;
        public int score1;
        public int score2;
        public int score3;
        public string username;
        public int rank;
    }

    private int mode = 1;

    [System.Serializable]
    public class PlayerDataArray
    {
        public UserData[] players;
    }

    [DllImport("__Internal")]
    private static extern void getUser(string path, string objectName, string username, string callback, string getRank);

    [DllImport("__Internal")]
    private static extern void getRankings(string path, string objectName,int mode, string callback);

    [DllImport("__Internal")]
    private static extern void listenForPlayerDataUpdates(string path, string objectName, string callback);


    private void Start()
    {
        getRankList();
    }

    public void getRankList()
    {
        listenForPlayerDataUpdates(path: "users", gameObject.name, callback: "fetchData");
    }

    public void fetchData()
    {
        string username = this.GetComponent<GameScript>().playerName;

        getRankings(path: "users", gameObject.name,mode:1, callback: "allRankings");

        if(this.GetComponent<GameScript>().PlayerMode)
        {
            getUser(path: "users", gameObject.name, username: username, callback: "getData", getRank: "myRankings");
        }else
        {
            this.GetComponent<GameScript>().playerRankText.text = "";
        }
    }

    private void getData(string data)
    {
        UserData userData = JsonUtility.FromJson<UserData>(data);

        int score = 0;

        if(mode == 1)
        {
            score = userData.score3;
        }else if(mode == 2)
        {
            score = userData.score2;
        }else if(mode == 3)
        {
            score = userData.score1;
        }    
        int rank = userData.rank;

        this.GetComponent<GameScript>().playerRankText.text = "CURRENT RANK "+ rank + "     " + this.GetComponent<GameScript>().playerName + "     (" + score + " pts)";
    }

    private void allRankings(string data)
    {

        string jsonString = data;

        PlayerDataArray playerDataArray = JsonUtility.FromJson<PlayerDataArray>("{\"players\":" + jsonString + "}");

        int i = 0;
        foreach (UserData playerData in playerDataArray.players)
        {
            Debug.Log("Name: " + playerData.name);
            Debug.Log("Password: " + playerData.password);
            Debug.Log("Score1: " + playerData.score1);
            Debug.Log("Score2: " + playerData.score2);
            Debug.Log("Score3: " + playerData.score3);
            Debug.Log("Username: " + playerData.username);

            if(playerData.username == this.GetComponent<GameScript>().playerName )
            {
                this.GetComponent<GameScript>().rankingText[i].color = Color.green;
                this.GetComponent<GameScript>().pointsText[i].color = Color.green;
                this.GetComponent<GameScript>().rankText[i].color = Color.green;
            }
            else
            {
                this.GetComponent<GameScript>().rankingText[i].color = Color.white;
                this.GetComponent<GameScript>().pointsText[i].color = Color.white;
                this.GetComponent<GameScript>().rankText[i].color = Color.white;
            }

            this.GetComponent<GameScript>().rankingText[i].text = playerData.name;
            

            if(mode == 1)
            {
                this.GetComponent<GameScript>().pointsText[i].text = playerData.score3.ToString()+ " pts";
            }else if(mode == 2)
            {
                this.GetComponent<GameScript>().pointsText[i].text = playerData.score2.ToString() + " pts";
            }else if( mode == 3)
            {
                this.GetComponent<GameScript>().pointsText[i].text = playerData.score1.ToString() + " pts";
            }
            i++;
        }

    }

    public void getRankingbyMode(int index)
    {
        this.GetComponent<GameScript>().playAudio(2);
        mode = index;
        if(this.GetComponent<GameScript>().PlayerMode)
        {
            //resetRankings();
            getRankings(path: "users", gameObject.name,mode:index, callback: "allRankings");
            getUser(path: "users", gameObject.name, username: this.GetComponent<GameScript>().playerName, callback: "getData", getRank: "myRankings");

            if (mode == 1)
            {
                this.GetComponent<GameScript>().modeText.text = "MEDIUM";
            }else if (mode == 2)
            {
                this.GetComponent<GameScript>().modeText.text = "HARD";
            }else if (mode == 3)
            {
                this.GetComponent<GameScript>().modeText.text = "MEGA";
            }
        }
    }   
    
    private void resetRankings()
    {
        for (int i = 0; i <= 9; i++)
        {
            this.GetComponent<GameScript>().rankingText[i].color = Color.white;
            this.GetComponent<GameScript>().pointsText[i].color = Color.white;
            this.GetComponent<GameScript>().rankText[i].color = Color.white;
            this.GetComponent<GameScript>().rankingText[i].text = "N/A";
            this.GetComponent<GameScript>().pointsText[i].text = "0 pts";
        }
    }





}
