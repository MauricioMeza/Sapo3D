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
    public bool connected;


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
        UnityWebRequest www = UnityWebRequest.Get("https://sapo-backend.herokuapp.com/score");
        www.certificateHandler = new SSLHandler();
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            connected = false;
        }
        else
        {
            //Extract JSON Data from response  
            puntajeLista = JsonUtility.FromJson<PuntajeLista>(www.downloadHandler.text);
            puntajeData = puntajeLista.data;
            connected = true;
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
        UnityWebRequest www = UnityWebRequest.Put("https://sapo-backend.herokuapp.com/addScore", JsonUtility.ToJson(ptj));
        www.certificateHandler = new SSLHandler();
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
            print(puntajeData);

            //Set Puntajes Data on All Text
            for (int i = 0; i < puntajeData.Count; i++)
            {
                scoreNamesText[i].text = puntajeData[i].name;
                scorePtsText[i].text = puntajeData[i].pts.ToString();
            }
        }
    }
}
