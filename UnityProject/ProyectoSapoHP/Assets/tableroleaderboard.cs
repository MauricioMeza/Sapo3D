using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class tableroleaderboard : MonoBehaviour
{
    private PuntajeLista puntajeLista;

    public List<GameObject> scoreNames;
    public List<GameObject> scorePts;
    private List<TextMesh> scoreNamesText;
    private List<TextMesh> scorePtsText;
    public List<Puntaje> puntajeData;

    private string nombre;
    private int puntos;


    // Start is called before the first frame update
    void Start()
    {
        //Link Text Gameobjects with ists Text Components
        scoreNamesText = new List<TextMesh>();
        scorePtsText= new List<TextMesh>();
        for(int i = 0; i < scoreNames.Count; i++) 
        {
            scoreNamesText.Add( scoreNames[i].GetComponent<TextMesh>() );
            scorePtsText.Add( scorePts[i].GetComponent<TextMesh>() );
        }

        //Launch Get Request
        StartCoroutine(GetScore());      
    }


    public void setScore(string n, int p) 
    {
        nombre = n;
        puntos = p;

        //Launch Post Request
        StartCoroutine(PostScore());   
    }

    //GET HTTP request
    IEnumerator GetScore() 
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.1:3000/score");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Extract JSON Data from response  
            puntajeLista = JsonUtility.FromJson<PuntajeLista>(www.downloadHandler.text);
            puntajeData = puntajeLista.data;

            //Set Puntajes Data on All Text
            for (int i = 0; i < puntajeData.Count; i++) 
            {
                scoreNamesText[i].text = puntajeData[i].name;
                scorePtsText[i].text = puntajeData[i].pts.ToString();
            }
        }
    }

    //POST HTTP request
    IEnumerator PostScore()
    {
        Puntaje ptj = new Puntaje();
        ptj.name = nombre;
        ptj.pts = puntos;
        UnityWebRequest www = UnityWebRequest.Put("http://127.0.1:3000/addScore", JsonUtility.ToJson(ptj));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Extract JSON Data from response
            puntajeLista = JsonUtility.FromJson<PuntajeLista>(www.downloadHandler.text);
            puntajeData = puntajeLista.data;

            //Set Puntajes Data on All Text
            for (int i = 0; i < puntajeData.Count; i++)
            {
                scoreNamesText[i].text = puntajeData[i].name;
                scorePtsText[i].text = puntajeData[i].pts.ToString();
            }
        }
    }
}
