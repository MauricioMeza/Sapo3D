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
    
    private TextMeshProUGUI scoreTextMesh;
    private bool finished;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        finishCanvas.SetActive(false);
        scoreTextMesh = scoreText.GetComponent<TextMeshProUGUI>();
        finished = false;
        GameObject a = Instantiate(argollaPrefab, transform.position, transform.rotation) as GameObject;
    }

    // Update is called once per frame
    // Spawns new Argolla after MouseUp
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && count < 9)
        {
            GameObject a = Instantiate(argollaPrefab, transform.position, transform.rotation) as GameObject;
            count++;
        }
        else if (Input.GetMouseButtonUp(0) && count == 9)
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

    private void finishGame()
    {
        finishCanvas.SetActive(true);
        finished = true;
        scoreTextMesh.text = scorecolide.score.ToString();
    }
}
