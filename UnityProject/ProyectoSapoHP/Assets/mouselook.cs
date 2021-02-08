using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{

    public float mouseSens;
    Vector2 looking;

    float cameraY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getMouseInput();
        Cursor.lockState = CursorLockMode.Locked;
        look();
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
            Camera.main.fieldOfView = 20;
        }
        else
        {
            Camera.main.fieldOfView = 42;
        }
    }

    //Rotate from looking vector
    void look()
    {
        transform.eulerAngles = (Vector2)looking * mouseSens;
    }
}
