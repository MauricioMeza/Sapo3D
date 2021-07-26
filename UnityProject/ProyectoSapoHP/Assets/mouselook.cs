using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{

    public float mouseSens;
    public bool playerControl;
    public GameObject argolla;
    private bool zoom;
    Vector2 looking;

    float cameraY;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = true;
        zoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Selection of Camera Mode
        if (playerControl)
        {
            getMouseInput();
            look();
        }
        else 
        {
            cameraFollow();
        }

        //Selecion of Zoom
        if (zoom)
        {
            Camera.main.fieldOfView = 25;
        }
        else 
        {
            Camera.main.fieldOfView = 42;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    //Get inputs from mouse for camera rotations and zoom
    void getMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        looking.x -= mouseY;
        looking.y += mouseX;

        looking.y = Mathf.Clamp(looking.y, -20, 20);
        looking.x = Mathf.Clamp(looking.x, -20, 40);

        //Zoom when right mouse button
        if (Input.GetMouseButton(1))
        {
            zoom = true;  
        }
        else
        {
            zoom = false;
        }
    }

    //Rotate from looking vector
    private void look()
    {
        transform.eulerAngles = (Vector2)looking * mouseSens;
    }

    private void cameraFollow()
    {
        transform.LookAt(argolla.transform);
    }


    public void changeCameraToFollow(GameObject argollaObj)
    {
        zoom = true;
        argolla = argollaObj;
        playerControl = false;
    }

    public void changeCameraToMouse()
    {
        zoom = false;
        playerControl = true;
    }
}
