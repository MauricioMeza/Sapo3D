using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class argollathrow : MonoBehaviour
{
    public GameObject camara;
    public GameObject argolla;
    private Rigidbody argollaRb;
    private AudioSource argollaSrc;  
    
    private float frontForce;
    private float upForce;
    private bool lanzado;
    private Vector3 posInicial;

    public AudioClip metal;
    public AudioClip cement;
    public AudioClip brick;
    public AudioClip wood;

    

    // Start is called before the first frame update
    void Awake()
    {
        camara = GameObject.Find("Main Camera");
        lanzado = false;
        argolla.transform.SetParent(camara.transform);
        argollaRb = argolla.GetComponent<Rigidbody>();
        argollaSrc = argolla.GetComponent<AudioSource>();
        argollaRb.useGravity = false;
        posInicial = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !lanzado)
        {
            shootArgolla();
            
        }

    }

    //Deparent the Argolla and give it initial force
    private void shootArgolla()
    {
        //calculate force from camera x rotation
        float x = 0;
        float y = 0;
        if (camara.transform.eulerAngles.x > 200)
        {
            x = (360 - camara.transform.eulerAngles.x) / 20;
            y = (360 - camara.transform.eulerAngles.x) / 40;
        }
        else
        {
            y = camara.transform.eulerAngles.x / 40;
            x = camara.transform.eulerAngles.x / 80;
        }
        float upForce = 2.0f + x;
        float frontForce = 2.0f + y;

        //Remove and activate child child while giving initial force
        Vector3 impulso = new Vector3(0.0f, frontForce, upForce);
        lanzado = true;
        argolla.transform.parent = null;
        argollaRb.useGravity = true;
        argollaRb.AddRelativeForce(impulso, ForceMode.Impulse);
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
