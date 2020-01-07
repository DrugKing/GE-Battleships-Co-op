using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreA;
    public static int scoreB;

    [SerializeField]
    private Text scoring;
    [SerializeField]
    private Text win;
    public GameObject winningpanel;

    // Start is called before the first frame update
    void Start()
    {
        winningpanel.SetActive(false);
        scoreA = 0;
        scoreB = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnBasedManager.turnNo == 1)
        {
            scoring.text = scoreA.ToString();
        }
        else
        {
            scoring.text = scoreB.ToString();
        }
        checkforwin();
        //To check if you can win
        if (Input.GetKeyDown("a"))
        {
            scoreB = 17;
        }
    }

    void checkforwin()
    {
        if(scoreA == 17)
        {
            winningpanel.SetActive(true);
            win.text = (PlayerNameInput.player1 + " win!");
        }
        if (scoreB == 17)
        {
            winningpanel.SetActive(true);
            win.text = (PlayerNameInput.player2 + " win!");
        }
    }
    public void Exit()
    {
        PlayerNameInput.player1 = null;
        PlayerNameInput.player2 = null;
        TurnBasedManager.instance.LeaveRoom();     
    }
}
