using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DragHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Camera PlayerCamera;
    [SerializeField]
    GameObject Controller;
    public static bool space;

    private bool notPlace;
    private float XPos =0.0f;
    private float ZPos =0.0f;
    private Quaternion turning = Quaternion.Euler(0, 90, 0);
    //public LayerMask Mask;

    Vector3 mousePoint = new Vector3(1.0f,0.5f,0.5f);
    void Update()
    {
        ShipPlacement();
        if (Input.GetMouseButtonDown(1) && notPlace)
        {
            Debug.Log("Pressed secondary button.");
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + turning.eulerAngles.y, 0);
        }
        if (Input.GetKeyDown("space"))
        {
            if (BoardManager.canplace == true && notPlace)
            {
                notPlace = false;
                space = true;
            }
        }
    }
    void Start()
    {
        notPlace = true;
        this.transform.position = mousePoint;
        if(TurnBasedManager.turnNo == 1)
        {
            Controller = GameObject.Find(PlayerNameInput.player1);
            PlayerCamera = Controller.GetComponentInChildren<Camera>();
        }
        if (TurnBasedManager.turnNo == 2)
        {
            Controller = GameObject.Find(PlayerNameInput.player2);
            PlayerCamera = Controller.GetComponentInChildren<Camera>();
        }
    }

    public void ShipPlacement()
    {
        if (notPlace)
        {            
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, layerMask: ~((1 << 8) | (1 << 9))))
            {
                GameObject hitObject = hit.transform.gameObject;
                XPos = (float)hit.transform.position.x;
                ZPos = (float)hit.transform.position.z;
            }
            mousePoint.x = XPos;
            mousePoint.z = ZPos;         
            this.transform.position = mousePoint;
        }
        else
        {
            return;
        }
        
    }
}
