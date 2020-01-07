using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject Controller;

    // Start is called before the first frame update
    void Start()
    {
        if (Controller.name == PlayerNameInput.player1)
        {
            Controller.GetComponentInChildren<Camera>().enabled = true;
        }
        if (Controller.name == PlayerNameInput.player2)
        {
            Controller.GetComponentInChildren<Camera>().enabled = false;
        }
        //transform.GetComponent<MovementController>().enabled = true;
    }

    void Update()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            if (Controller.name == PlayerNameInput.player1)
            {
                Controller.GetComponentInChildren<Camera>().enabled = true;
            }
            if (Controller.name == PlayerNameInput.player2)
            {
                Controller.GetComponentInChildren<Camera>().enabled = false;
            }
            //transform.GetComponent<MovementController>().enabled = true;
            //PlayerCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {

            if (Controller.name == PlayerNameInput.player1)
            {
                Controller.GetComponentInChildren<Camera>().enabled = false;
            }
            if (Controller.name == PlayerNameInput.player2)
            {
                Controller.GetComponentInChildren<Camera>().enabled = true;
            }
            //transform.GetComponent<MovementController>().enabled = false;
            //PlayerCamera.GetComponent<Camera>().enabled = false;
        }
    }
}