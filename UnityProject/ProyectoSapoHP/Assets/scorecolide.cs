using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handling of Scoring (when an argolla goes into a hole)
public class scorecolide : MonoBehaviour
{

    private AudioSource huecoSrc; 
    public int individualScore;
    public static int score;
    private TextMesh textObject;


    //----Start is called before the first frame update
    void Start()
    {
        //--Initialize score, score text and audio for soce entrance
        score = 0;
        textObject = GameObject.Find("TextScore").GetComponent<TextMesh>();
        textObject.text = score.ToString();
        huecoSrc = GetComponent<AudioSource>();
  
    }


    //----Action after a collision detected in the Holes
    void OnCollisionEnter(Collision col)
    {
        //--If argolla collides with hole Sum Score, Present it and get new argolla
        if(col.gameObject.name == "Argolla" || col.gameObject.name == "Argolla(Clone)") 
        {
            GameObject argolla = col.gameObject;
            score += individualScore;
            textObject.text = score.ToString();
            //In Case an argolla enters because of a rebound
            if (!argolla.GetComponent<argollathrow>().detenido)
            {
                argolla.GetComponent<argollathrow>().spawnNewArgolla();
            }
            Destroy(col.gameObject);
            huecoSrc.Play();
        }
    }

}
