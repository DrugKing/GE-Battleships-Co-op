using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNameInput : MonoBehaviour
{
    public static string player1;
    public static string player2;
    public void PLAYERNAME1(string playera)
    {
        if (string.IsNullOrEmpty(playera))
        {
            Debug.Log("NO name input yet");
            playera = null;
        }
//        PhotonNetwork.NickName = playera;
        player1 = playera;
        return;
    }

    public void PLAYERNAME2(string playerb)
    {
        if (string.IsNullOrEmpty(playerb))
        {
            Debug.Log("NO name input yet");
            playerb = null;
        }
        player2 = playerb;
        return;
    }
}
