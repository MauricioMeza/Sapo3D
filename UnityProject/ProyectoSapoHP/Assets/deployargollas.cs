using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//-----Handles the deployment and count of Argollas----
public class deployargollas : MonoBehaviour
{
    public GameObject argollaPrefab;
    public GameObject finishCanvas;
    public GameObject scoreText;
    public GameObject scoreTextMsg;
    public GameObject scoreCongrats;
    public GameObject scoreNameInput;
    public GameObject tablero;

    public static int count;
    public int final = 7;
    private GameObject argollaInstancia;
    
    private TextMeshProUGUI scoreTextMesh;
    private TextMeshProUGUI scoreTextMeshMsg;
    private InputField inputField;
    private tableroleaderboard tablerolead;
    private bool finished;



    void Start()
    {
        //--Initially no argollas have been thrown and we spawn the first--
        count = 0;
        spawnArgolla();

        finishCanvas.SetActive(false);
        finished = false;

        scoreCongrats.SetActive(false);
        scoreNameInput.SetActive(false);
        scoreTextMesh = scoreText.GetComponent<TextMeshProUGUI>();
        scoreTextMeshMsg = scoreTextMsg.GetComponent<TextMeshProUGUI>();
        inputField = scoreNameInput.GetComponent<InputField>();
        tablerolead = tablero.GetComponent<tableroleaderboard>();
    }


    //---Checks if all argollas have been thrown and shows end game screen---
    void Update()
    {
        //--Finish game if all argollas have been thrown
        if (count > final)
        {
            finishGame();
        }

        //--Shows score in text even after game is over (in case last one goes in
        if (finished)
        {
            scoreTextMesh.text = scorecolide.score.ToString();
            inputField.Select();
        }

        //--Restarts after player decides to
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //If name is Written send Post Request
            if (inputField.text != "") 
            {
                tablerolead.setScore(inputField.text, scorecolide.score);
            }
            SceneManager.LoadScene("SampleScene");      
        }
    }

    //----Spawns new Argolla if the games hasnt finished
    public void spawnArgolla()
    {
        if (count < final)
        {
            //Instantiates the argolla profab and counts the argolla
            argollaInstancia = Instantiate(argollaPrefab, transform.position, transform.rotation) as GameObject;
            count++;
        }
        else 
        {
            finishGame();
        }
    }


    //----Show End Game Text
    private void finishGame()
    {
        finishCanvas.SetActive(true);
        finished = true;
        scoreTextMesh.text = scorecolide.score.ToString();
        
        //--If there is a REST connection check if puntaje gets into leaderboard
        if (tablerolead.connected)
        {
            int puntajeEntrada = tablerolead.puntajeData[4].pts;
            if (scorecolide.score > puntajeEntrada)
            {
                //SetUp UI for leaderboard POST request
                scoreCongrats.SetActive(true);
                scoreNameInput.SetActive(true);
                scoreTextMeshMsg.text = "Escribe tu nombre y presiona Enter";
            }
        }
    }
}
