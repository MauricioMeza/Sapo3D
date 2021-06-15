using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class argollathrow : MonoBehaviour
{
    private GameObject camara;
    private GameObject spawnPoint;
    public GameObject argolla;
    private Rigidbody argollaRb;
    private AudioSource argollaSrc;
    private bool lanzado;
    
    public AudioClip metal;
    public AudioClip cement;
    public AudioClip brick;
    public AudioClip wood;

    public LineRenderer linea;
    private Vector3[] puntosSim; 
    private float m;
    private float g;
    private Vector3 p0;

    // Start is called before the first frame update
    void Awake()
    {
        camara = GameObject.Find("Main Camera");
        spawnPoint = GameObject.Find("SpawnPoint");
        lanzado = false;
        argolla.transform.SetParent(camara.transform);
        argollaRb = argolla.GetComponent<Rigidbody>();
        argollaSrc = argolla.GetComponent<AudioSource>();
        argollaRb.useGravity = false;

        puntosSim = new Vector3[10];
        m = argollaRb.mass;
        g = -Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lanzado)
        {
            Vector3[] vectorApuntar = vectorCalc();
            float fuerza = 10;
            simulateArgolla(vectorApuntar[1], fuerza);
            if (Input.GetMouseButtonDown(0))
            {
                shootArgolla(vectorApuntar[0], fuerza);
            }
        }       
    }

    //Caculate the vector of shooting from current camera position
    private Vector3[] vectorCalc() 
    {
        Vector3 argollaPosicion = argolla.transform.position;
        float camX = camara.transform.eulerAngles.x;
        float camY = camara.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(camX, camY, 0);
    
        //Vector for shooting

        Vector3 forward = Vector3.forward + (Vector3.up * 0.25f);
        //Vector for initial
        Vector3 apuntarPosicion = rotation * forward;
        Vector3 drawPosicion = (apuntarPosicion*5) + argollaPosicion;

        Vector3[] vectorList = new Vector3[3]{ forward, apuntarPosicion, drawPosicion };

        Debug.DrawLine(argollaPosicion, drawPosicion);
        return vectorList;
    }

    //Deparent the Argolla and give it initial force
    private void shootArgolla(Vector3 vectorApuntar, float fuerza)
    {
        lanzado = true;
        argolla.transform.parent = null;
        argollaRb.useGravity = true;
        argollaRb.AddRelativeForce(vectorApuntar * fuerza, ForceMode.Impulse);
    }

    //Calculate parabolic movement of the Argolla
    private void simulateArgolla(Vector3 vectorApuntar, float f)
    {
        p0 = argolla.transform.position;
        puntosSim[0] = p0;
        vectorApuntar = vectorApuntar;

        print(vectorApuntar);

        float t = 0;
        for (int i = 1; i < puntosSim.Length; i++)
        {
            t = (float)i / 20.0f;
            float dz = f * t * vectorApuntar.z;
            float dx = f * t * vectorApuntar.x;
            float dy = (vectorApuntar.y * t * f) - (g * t*t / 2) ;

            Vector3 pos = new Vector3(p0.x + dx, p0.y + dy, p0.z + dz - 0.05f);
            puntosSim[i] = pos;
        }

        string result = "";
        for (int i = 0; i < puntosSim.Length; i++) 
        {
            result += puntosSim[i].ToString() + ", ";
        }

        //print(p0);
        //print(result);

        spawnPoint.GetComponent<linePaint>().LinePaint(puntosSim);
    }
    

    //Sound handling for the argolla collision
    void OnCollisionEnter(Collision col)
    {
        float force = argollaRb.velocity.magnitude;

        if (force >= 0)
        {
            switch (col.gameObject.name)
            { 
                case "Base":
                    argollaSrc.PlayOneShot(wood, force / 2);
                    //Light wood hit sound
                    break;
                case "Blanco":
                    argollaSrc.PlayOneShot(wood, force / 2);
                    //Heavier wood hit sound
                    break;
                case "Sapo":
                    argollaSrc.PlayOneShot(metal, force / 2);
                    //Heavy metal sound
                    break;
                case "Argolla":
                case "Argolla(Clone)":
                    argollaSrc.PlayOneShot(metal, force / 2);
                    //Lighter metal sound
                    break;
                case "Pared":
                    //Light cement sound;
                    argollaSrc.PlayOneShot(brick, force / 2);
                    break;
                case "Piso":
                    argollaSrc.PlayOneShot(cement, force / 2);
                    //Heavier cement sound;
                    break;
            }
            
        }
    }

}
