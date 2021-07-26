using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class deployargollas : MonoBehaviour
{
    public GameObject argollaPrefab;
    public GameObject finishCanvas;
    public GameObject scoreText;

    public static int count;
    public int final = 7;
    
    private TextMeshProUGUI scoreTextMesh;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        finishCanvas.SetActive(false);
        scoreTextMesh = scoreText.GetComponent<TextMeshProUGUI>();
        finished = false;
        spawnArgolla();
        
    }

    // Update is called once per frame
    //Checks if all argollas have been thrown and shows end game screen
    void Update()
    {
        if (count > final)
        {
            finishGame();
        }

        if (finished)
        {
            scoreTextMesh.text = scorecolide.score.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("SampleScene");      
        }
    }

    // Spawns new Argolla
    public void spawnArgolla()
    {
        if (count < final)
        {
            GameObject a = Instantiate(argollaPrefab, transform.position, transform.rotation) as GameObject;
            count++;
            print(count);
        }
        else 
        {
            finishGame();
        }
    }

    private void finishGame()
    {
        finishCanvas.SetActive(true);
        finished = true;
        scoreTextMesh.text = scorecolide.score.ToString();
    }
}
