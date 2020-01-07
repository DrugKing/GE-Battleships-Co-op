using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LauncherManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public GameObject GameLauncherPanel;
    public GameObject LobbyPanel;
    public GameObject ErrorPanel;
    public GameObject NoRoom;
    public GameObject NoName;


    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        GameLauncherPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        ErrorPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    #endregion

    #region Public Methods
    public void ConnectToMainServer()
    {
        bool namechecking = (PlayerNameInput.player1 == null || PlayerNameInput.player2 == null);
        if (namechecking)
        {
            ErrorPanel.SetActive(true);
            NoName.SetActive(true);
            NoRoom.SetActive(false);
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0, 100);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 7;
        //how long can player reconnect, milliseconds they said...
        roomOptions.PlayerTtl = 5 * 1000;
        roomOptions.PublishUserId = true;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    public void Close()
    {
        ErrorPanel.SetActive(false);
    }

    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
         Debug.Log("Bezerker's Shit is Coming!!");
         GameLauncherPanel.SetActive(false);
         LobbyPanel.SetActive(true);
    }

    public override void OnConnected()
    {
        Debug.Log("Internet Getto");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        ErrorPanel.SetActive(true);
        NoName.SetActive(false);
        NoRoom.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Battlefield");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }


    #endregion

    #region Private Methods



    #endregion
}
