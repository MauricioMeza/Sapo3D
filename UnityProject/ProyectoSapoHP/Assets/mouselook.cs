using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handling of mouselook movement and camera movement
public class mouselook : MonoBehaviour
{

    public float mouseSens;
    public bool playerControl;
    public GameObject argolla;
    private bool zoom;
    Vector2 looking;
    float cameraY;


    void Start()
    {
        //Controls start in mouselook player control
        init();
         
    }
    public void init() 
    {
        playerControl = true;
        zoom = false;
        looking.x = 10;
        looking.y = 0;
    }



    void Update()
    {
        //--Selection of Camera Mode
        if (playerControl)
        {
            //Camera From mouse input
            getMouseInput();
            transform.eulerAngles = (Vector2)looking * mouseSens;
        }
        else 
        {
            //Camera Follows the Argolla
            transform.LookAt(argolla.transform);
        }


        //--Selecion of Zoom
        if (zoom)
        {
            Camera.main.fieldOfView = 30;
        }
        else 
        {
            Camera.main.fieldOfView = 42;
        }
        
        //Makes sure cursor is locked
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    //----Get inputs from mouse for camera rotations and zoom
    void getMouseInput()
    {
        //--X,Y axis of mouse movement in the frame are added to a normalized Vector2
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        looking.x -= mouseY;
        looking.y += mouseX;

        looking.y = Mathf.Clamp(looking.y, -20, 20);
        looking.x = Mathf.Clamp(looking.x, -20, 40);


        //--Zoom when right mouse button
        if (Input.GetMouseButton(1))
        {
            zoom = true;  
        }
        else
        {
            zoom = false;
        }
    }

    
    //----Change to Camera follow and Zoom in with a slight delay (Called when Argolla is thrown) 
    public void changeCameraToFollow(GameObject argollaObj)
    {
        StartCoroutine(follow(argollaObj));   
    }
    IEnumerator follow(GameObject argollaObj)
    {
        yield return new WaitForSeconds(0.12f);
        zoom = true;
        argolla = argollaObj;
        playerControl = false;
    }

    //----Give Player Control and Zoom Out (Called when Argolla Stops)
    public void changeCameraToMouse()
    {
        zoom = false;
        playerControl = true;
    }
}

      