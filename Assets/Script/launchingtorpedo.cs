using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchingtorpedo : MonoBehaviour
{
    public GameObject[] PlayerLauncher;
    public float speed = 20.0f;
    public GameObject Launcher;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLauncher = GameObject.FindGameObjectsWithTag("Launch");
        Checkwhoshoot();        
    }

    // Update is called once per frame
    void Update()
    {         

    }

    void Checkwhoshoot()
    {
        for (int i = 0; i < PlayerLauncher.Length; i++)
        {
            Transform Parent = PlayerLauncher[i].GetComponentInParent<Transform>();
            Debug.Log("player 1: " + (TurnBasedManager.turnNo == 1 && Parent.parent.name == PlayerNameInput.player1));
            Debug.Log("player 2: " + (TurnBasedManager.turnNo == 2 && Parent.parent.name == PlayerNameInput.player2));
            if (TurnBasedManager.turnNo == 1 && Parent.parent.name == PlayerNameInput.player1)
            {
                Launcher = PlayerLauncher[i];
                //this.transform.position = PlayerLauncher[i].transform.position;                
            }
            if (TurnBasedManager.turnNo == 2 && Parent.parent.name == PlayerNameInput.player2)
            {
                Launcher = PlayerLauncher[i];
            }       
        }
        transform.SetParent(Launcher.transform);
        Vector3 target = new Vector3(BoardManager.SelectionX, -5.0f, BoardManager.SelectionZ);
        this.transform.position = Vector3.MoveTowards(Launcher.transform.position, target, Time.deltaTime * speed);
    }
}         