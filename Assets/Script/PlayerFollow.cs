using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField]
    Camera PlayerCameras;
    [SerializeField]
    GameObject Controller;

    float mainSpeed = 50.0f; 
    float shiftAdd = 250.0f; 
    float maxShift = 1000.0f; 
    float camSens = 0.25f; 
    private Vector3 lastMouse = new Vector3(255, 255, 255); 
    private float totalRun = 1.0f;

    void Start()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            Controller = GameObject.Find(PlayerNameInput.player1);
            PlayerCameras = Controller.GetComponentInChildren<Camera>();
        }
        if (TurnBasedManager.turnNo == 2)
        {
            Controller = GameObject.Find(PlayerNameInput.player2);
            PlayerCameras = Controller.GetComponentInChildren<Camera>();
        }
    }
    void Update()
    {
        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        p = p * mainSpeed;
       

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { 
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { 
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}


    
      

    
    
