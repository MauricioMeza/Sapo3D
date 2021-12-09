using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class argollathrow : MonoBehaviour
{
    private GameObject camara;
    private GameObject spawnPoint;
    public GameObject argolla;
    private GameObject thrownArgollas;
    private Rigidbody argollaRb;
    private AudioSource argollaSrc;
    private bool lanzado;
    public bool detenido;
    private bool finalizado;
    private float airTime;
    
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
        thrownArgollas = GameObject.Find("ThrownArgollas");
        
        argollaRb = argolla.GetComponent<Rigidbody>();
        argollaSrc = argolla.GetComponent<AudioSource>();

        init();

        m = argollaRb.mass;
        g = -Physics.gravity.y;
    }
    public void init() 
    {
        lanzado = false;
        detenido = false;
        airTime = 0;
        argolla.transform.SetParent(camara.transform);
        argollaRb.useGravity = false;
        puntosSim = new Vector3[10];
    }

    // Update is called once per frame
    void Update()
    {
        //If argolla hasnt been thrown calculate the line;
        if (!lanzado)
        {
            Vector3[] vectorApuntar = vectorCalc();
            float fuerza = 10;
            simulateArgolla(vectorApuntar[1], fuerza);
            
            //If press button, throw argolla
            if (Input.GetMouseButtonDown(0))
            {
                shootArgolla(vectorApuntar[0], fuerza);

            }
        }
        
        if(lanzado && !detenido)
        {
            //If argolla has been thrown check the speed to see if its still moving to spawn new argolla
            float vel = argollaRb.velocity.magnitude;
            airTime += Time.deltaTime;
            if (vel < 0.1 && airTime > 0.1)
            {
                spawnNewArgolla();
            }
        }
    }

    //Deparent the Argolla and give it initial force
    private void shootArgolla(Vector3 vectorApuntar, float fuerza)
    {
        camara.GetComponent<mouselook>().changeCameraToFollow(argolla);
        lanzado = true;
        argolla.transform.parent = thrownArgollas.transform;
        argollaRb.useGravity = true;
        argollaRb.AddRelativeForce(vectorApuntar * fuerza, ForceMode.Impulse);
    }

    //Call spawn point spawnArgolla method
    public void spawnNewArgolla() 
    {
        camara.GetComponent<mouselook>().changeCameraToMouse();
        detenido = true;
        airTime = 0;
        spawnPoint.GetComponent<deployargollas>().spawnArgolla();
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

    //----Calculate parabolic movement of the Argolla trajectory prediction
    private void simulateArgolla(Vector3 vectorApuntar, float f)
    {
        //Get Inital Position
        p0 = argolla.transform.position;
        puntosSim[0] = p0;

        //For every point in the array calcualte its trajectotry
        float t = 0;
        for (int i = 1; i < puntosSim.Length; i++)
        {
            //In every time step of 0.05s calculate position of X,Y,Z Vectors as Time*Velocity with Y Vector being affected by gravity 
            t = (float)i / 20.0f;
            float dz = f * t * vectorApuntar.z;
            float dx = f * t * vectorApuntar.x;
            float dy = (f * t * vectorApuntar.y) - (g * t*t / 2) ;

            //Generate Vector3 For prediction of position in space
            Vector3 pos = new Vector3(p0.x + dx, p0.y + dy, p0.z + dz - 0.05f);
            puntosSim[i] = pos;
        }

        //In case we want to print the prediction
        /* 
        string result = "";
        for (int i = 0; i < puntosSim.Length; i++) 
        {
            result += puntosSim[i].ToString() + ", ";
        }
        print(result);
        */

        spawnPoint.GetComponent<linePaint>().LinePaint(puntosSim);
    }
    

    //----Sound handling for the argolla collision
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
