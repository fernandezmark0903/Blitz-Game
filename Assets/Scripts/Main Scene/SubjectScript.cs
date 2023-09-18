using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectScript : MonoBehaviour
{
    [SerializeField]
    public Sprite[] subjectImages;
    public int PositionX;
    public GameObject gameManager;

    private int rand = 0;
    public string answer = "";
    GameScript gm = null;
    

    void Start()
    {
        gm = gameManager.GetComponent<GameScript>();
        generateSubject();
    }


    public void generateSubject()
    {
        gameObject.SetActive(true);
        rand = UnityEngine.Random.Range(0, 8);
        gameObject.GetComponent<Image>().sprite = subjectImages[rand];

        //Debug.Log(rand);
        if (rand > 3)
        {
            //blue
            //Debug.Log("Yellow");
            gameObject.transform.localPosition = new Vector3((gameObject.transform.localPosition.x - PositionX), 0, 0);
        }
        else

        {
            //yellow
            gameObject.transform.localPosition = new Vector3((gameObject.transform.localPosition.x + PositionX), 0, 0);
            //Debug.Log("Blue");
        }


        switch (rand)
        {
                case 0:
                    answer = "yellow";
            break;
                case 1:
                    answer = "yellow";
            break;
                case 2:
                    answer = "blue";
            break;
                case 3:
                    answer = "blue";
            break;
                case 4:
                    answer = "blue";
            break;
                case 5:
                    answer = "yellow";
            break;
                case 6:
                    answer = "blue";
            break;
                case 7:
                    answer = "yellow";
            break;

        }

        //Debug.Log(answer);
        
        if(gm != null)
        {
            gm.playAudio(0);
        }
        

    }





    
}
