using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void updateScoreByUsername(string path, string username, int score,int mode);

    public AudioSource audioSource;

    public AudioClip[] audioClipArray;

    public GameObject UIButtons;
    public GameObject subject;
    public GameObject gameUI;
    public GameObject gameStats;
    public GameObject timerUI;

    public Text scoreText;
    public Text scoreStats;
    public Text bestStats;
    public Text scoreTextOnline;
    public Text timerText;
    public Text playerNameText;
    public Text playerRankText;

    public GameObject[] gamestatsText;

    public GameObject lifeAmount;
    public GameObject homeButton;
    public GameObject[] gameLayer;
    public GameObject leadeboardPanel;

    public Text[] rankingText;
    public Text[] pointsText;
    public Text[] rankText;
    public Text modeText;

    public GameObject timerBar;
    public GameObject timerBarBG;

    public Dropdown timerCount;

    SubjectScript ss = null;
    private int gameStatus = 0;
    private int scoreValue = 0;
    private int gameMode = 0;
    private int timerValue = 0;
    private int timerValueN = 0;
    private int timerCounter = 30;

    private bool addminusLife = true;

    public string playerName = "";

    public bool PlayerMode = false;

    public float totalTime = 10.0f; // Total time in seconds
    private float currentTime = 0.0f;

    private bool gameStart = false;
    private bool menuPress = false;


    void Start()
    {
        ss = subject.GetComponent<SubjectScript>();
        getData();
        getTimer();
    }

    void Update()
    {
        if (gameStart)
        {
            gameStats.SetActive(false);
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                float fillAmount = currentTime / totalTime;
                timerBar.GetComponent<Image>().fillAmount = fillAmount;
            }
            else
            {
                if (gameMode == 0 || gameMode == 1 || gameMode == 2)
                {
                    currentTime = 0;
                    gameOver();
                    return;
                }
            }
        }
    }

    public void buttonClick(int index)
    {
        if (gameStatus == 0) return;

        if (ss.answer == "yellow")
        {
            if (index == 0)
            {
                //Debug.Log("CORRECT");
                addPoint();
                ss.generateSubject();
            }
            else
            {
                //Debug.Log("WRONG");
                playAudio(1);
                if (gameMode == 0 || gameMode == 1 || gameMode == 2)
                {
                    gameOver();
                    return;
                }

                Image imgLife = lifeAmount.GetComponent<Image>();
                if (imgLife.fillAmount <= 0)
                {
                    gameOver();
                    return;
                }
                else
                {
                    imgLife.fillAmount = imgLife.fillAmount - 0.34f;
                }

                addminusLife = false;
                ss.generateSubject();


            }
        }
        else if (ss.answer == "blue")
        {
            if (index == 1)
            {
                //Debug.Log("CORRECT");
                addPoint();
                ss.generateSubject();
            }
            else
            {
                //Debug.Log("WRONG");
                playAudio(1);
                if (gameMode == 0 || gameMode == 1 || gameMode == 2)
                {
                    gameOver();
                    return;
                }

                Image imgLife = lifeAmount.GetComponent<Image>();
                if (imgLife.fillAmount <= 0)
                {
                    gameOver();
                    return;
                }else
                {
                    imgLife.fillAmount = imgLife.fillAmount - 0.34f;
                }

                
                addminusLife = false;
                ss.generateSubject();
                
            }
        }

        timerValueN = 10;
    }

    public void startGame(int index)
    {
        timerValueN = 10;
        scoreValue = 0;
        gameStatus = 1;
        UIButtons.SetActive(false);
        subject.SetActive(true);
        gameUI.SetActive(true);
        gameMode = index;
        gameStart = true;
        totalTime = float.Parse(timerCount.options[timerCount.value].text);

        if (index == 0 || index == 1 || index == 2)
        {
            StartCoroutine(normalModeTimer());

            if (index == 0) timerValue = 1;
            else if (index == 1) timerValue = 5;
            else if (index == 2) timerValue = 3;
            timerUI.SetActive(true);
            gameLayer[0].SetActive(false);
            gameLayer[1].SetActive(false);
            gameLayer[2].SetActive(true);
            currentTime = totalTime;
            timerBar.SetActive(true);
            timerBarBG.SetActive(true);


        }
        else if (index == 3)
        {
            timerValue = timerCounter;
            timerUI.SetActive(true);
            StartCoroutine(survivalMode());
            StartCoroutine(normalMode());
            gameLayer[0].SetActive(true);
            gameLayer[1].SetActive(true);
            gameLayer[2].SetActive(false);

            timerBar.SetActive(false);
            timerBarBG.SetActive(false);
        }


        timerText.text = Math.Abs(timerValue).ToString();
        subject.GetComponent<GlideController>().SetDestination(0);
        subject.GetComponent<SubjectScript>().generateSubject();

        playAudio(2);
        homeButton.SetActive(true);
        Image imgLife = lifeAmount.GetComponent<Image>();
        imgLife.fillAmount = 1;
    }

    public void reStartgame()
    {
        
        scoreText.text = scoreValue.ToString();
        gameStats.SetActive(false);
        subject.GetComponent<GlideController>().SetDestination(0);
        subject.GetComponent<SubjectScript>().generateSubject();
        startGame(gameMode);
    }

    public void gameOver()
    {
        gameStart = false;
        timerBar.SetActive(false);
        timerBarBG.SetActive(false);
        currentTime = 0;

        gameStatus = 0;

        scoreStats.text = scoreValue.ToString();
        scoreTextOnline.text = scoreValue.ToString();

        if (PlayerPrefs.GetInt("best"+gameMode) == 0)
        {
            PlayerPrefs.SetInt("best"+ gameMode, scoreValue);
        }else if(scoreValue >= PlayerPrefs.GetInt("best" + gameMode))
        {
            PlayerPrefs.SetInt("best"+ gameMode, scoreValue);
        }

        bestStats.text = PlayerPrefs.GetInt("best"+ gameMode).ToString();



        subject.GetComponent<Animation>().Play();
        playAudio(1);
        subject.GetComponent<GlideController>().SetDestination(1);
        homeButton.SetActive(false);
        
        StartCoroutine(showRestart());

        if(PlayerMode)
        {

            int mode = 1;

            if(gameMode == 1)
            {
                mode = 3;
            }else if(gameMode == 2)
            {
                mode = 2;
            }else if(gameMode == 3)
            {
                mode = 1; ;
            }
            updateScoreByUsername(path: "users", username: playerName,score: scoreValue,mode: mode);
        }
    }

    public void playAudio(int index)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = audioClipArray[index];
            audioSource.Play();
        }
    }

    public void buttonManager(int index)

    {   if(index == 1)
        {
            Screen.fullScreen = true;
        }else if(index == 2)
        {
            UIButtons.SetActive(true);
            gameStats.SetActive(false);
        }else if(index == 3)
        {
            string link = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fwww.drumattackschool.com%2F&hashtag=%23Blitz";
            Application.ExternalEval("window.open('" + link + "', '_blank')");
        }
        else if (index == 4)
        {
            string link = "https://twitter.com/intent/tweet?url=https%3A%2F%2Fwww.drumattackschool.com%2F&text=Blitz";
            Application.ExternalEval("window.open('" + link + "', '_blank')");
        }
        else if (index == 5)
        {
            string link = "https://api.whatsapp.com/send/?text=Blitz%20Game%0D%0Ahttps%3A%2F%2Fwww.drumattackschool.com&type=custom_url&app_absent=0";
            Application.ExternalEval("window.open('" + link + "', '_blank')");
        }
        else if(index == 6)
        {
            SceneManager.LoadScene("LoginScene");
        }
        else if(index == 7)
        {
            leadeboardPanel.SetActive(true);
        }else if(index == 8)
        {
            leadeboardPanel.SetActive(false);
        }

        playAudio(2);
    }

    public void addPoint()
    {
        scoreValue++;
        scoreText.text = scoreValue.ToString();
        if (gameMode == 3)
        {
            timerText.text = Math.Abs(timerValue).ToString();
        }
        else
        {
            resetTime(gameMode);
        }
    }

    public void resetTime(int gameMode)
    {
        int[] time = { 1, 5, 3 };
        timerValue = time[gameMode];
        timerText.text = Math.Abs(timerValue).ToString();
    }

    public void lifeUpdate(float lifeValue)
    {

        addminusLife = true;
        Image imgLife = lifeAmount.GetComponent<Image>();

        if (imgLife.fillAmount <=0f)
        {
            timerText.text = "0";
            gameOver();
        }else
        {
            imgLife.fillAmount = imgLife.fillAmount + lifeValue;
            timerValue = timerCounter;
            timerText.text = timerValue.ToString();
            StartCoroutine(survivalMode());
        }
    }


    public void gameMenu()
    {
        gameStatus = 0;

        scoreStats.text = scoreValue.ToString();
        scoreTextOnline.text = scoreValue.ToString();

        if (PlayerPrefs.GetInt("best" + gameMode) == 0)
        {
            PlayerPrefs.SetInt("best" + gameMode, scoreValue);
        }
        else if (scoreValue >= PlayerPrefs.GetInt("best" + gameMode))
        {
            PlayerPrefs.SetInt("best" + gameMode, scoreValue);
        }

        bestStats.text = PlayerPrefs.GetInt("best" + gameMode).ToString();

        subject.GetComponent<Animation>().Play();
        playAudio(1);
        subject.GetComponent<GlideController>().SetDestination(1);
        menuPress = true;
        //UIButtons.SetActive(true);

        timerBar.SetActive(false);
        timerBarBG.SetActive(false);
        currentTime = 0;

        if(gameMode == 3)
        {
            StartCoroutine(showRestart());
        }
    }

    private void getData()
    {
        playerName = PlayerPrefs.GetString("playerName");

        if (playerName == "")
        {
            playerNameText.text = "NAME: GUEST";
            PlayerMode = false;
        }
        else
        {
            playerNameText.text = "NAME: " + playerName;
            PlayerMode = true;
        }
    }

    public void getTimer()
    {
        timerCount.ClearOptions();

        List<string> timerCounts = new List<string>();
        int fixVal = 15;
        for (int i = 1; i <= 5; i++)
        {
            timerCounts.Add((fixVal * i).ToString());
        }
        timerCount.AddOptions(timerCounts);
    }


    IEnumerator showRestart()
    {
        yield return new WaitForSeconds(2);
        
        if(menuPress)
        {
            UIButtons.SetActive(true);
            gameUI.SetActive(false);
            timerUI.SetActive(false);
            gameStats.SetActive(false);
        }
        else
        {
            UIButtons.SetActive(false);
            gameUI.SetActive(false);
            timerUI.SetActive(false);
            gameStats.SetActive(true);

            if (PlayerMode)
            {
                gamestatsText[0].SetActive(false);
                gamestatsText[1].SetActive(false);
                gamestatsText[2].SetActive(false);
                gamestatsText[3].SetActive(false);
                gamestatsText[4].SetActive(true);
                gamestatsText[5].SetActive(true);
            }else
            {
                gamestatsText[0].SetActive(true);
                gamestatsText[1].SetActive(true);
                gamestatsText[2].SetActive(true);
                gamestatsText[3].SetActive(true);
                gamestatsText[4].SetActive(false);
                gamestatsText[5].SetActive(false);
            }
        }

        menuPress = false;
        scoreValue = 0;
        scoreText.text = scoreValue.ToString();
    }

    IEnumerator survivalMode()
    {
        yield return new WaitForSeconds(1);
        timerValue--;
        timerText.text = timerValue.ToString();

        if(timerValue > -1)
        {
            if (gameStatus == 0)
            {
                StopCoroutine(survivalMode());
            }
            else if(gameStatus == 1)
            {
                StartCoroutine(survivalMode());
            }
        }
        else
        {
            if(addminusLife)
            {
                lifeUpdate(0.34f);
            }
            else
            {
                lifeUpdate(-0.34f);
            }
        }
    }

    IEnumerator normalMode()
    {
        yield return new WaitForSeconds(1);
        timerValueN--;
        if (timerValueN > -1)
        {
            if (gameStatus == 0)
            {
                StopCoroutine(normalMode());
            }
            else if (gameStatus == 1)
            {
                StartCoroutine(normalMode());
            }
        }
        else
        {
            gameOver();
        }
    }

    IEnumerator normalModeTimer()
    {
        yield return new WaitForSeconds(1);
        timerValue--;
        timerText.text = Math.Abs(timerValue).ToString();
        if (timerValue > -1)
        {
            if (gameStatus == 0)
            {
                StopCoroutine(normalModeTimer());
            }
            else if (gameStatus == 1)
            {
                StartCoroutine(normalModeTimer());
            }
        }
        else
        {
            timerText.text = "0";
            gameOver();
        }
    }

}
