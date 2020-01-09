using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class TurnBasedManager : MonoBehaviourPunCallbacks//, IOnEventCallback
{
    [SerializeField]
    Camera PlayerCamera;

    [SerializeField]
    private Text playername;

    [SerializeField]
    private Text nextplayer;

    string localplayer = "";
    string remoteplayer = "";
    string localname = "";
    string remotename = "";

    [SerializeField]
    GameObject playerPrefab;
    public GameObject StartPanel;
    public GameObject torpedo;

    public static TurnBasedManager instance;
    public static int turnNo = 1;
    public static bool strike = false;
    public static bool miss = false;
    GameObject p1;
    GameObject p2;
    int layerMask;    

    #region Unity Methods
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UniqueRoom();
        if (PhotonNetwork.IsConnected)
        {
            if (playerPrefab != null)
            {
                int spawnPoint = 5;
                p1=Instantiate(playerPrefab, new Vector3(spawnPoint, 5, -spawnPoint), Quaternion.identity);
                p1.name = PlayerNameInput.player1;
                Quaternion rotate = Quaternion.Euler(0, 180, 0);
                p2=Instantiate(playerPrefab, new Vector3(spawnPoint, 5, spawnPoint+30), rotate);
                p2.name = PlayerNameInput.player2;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {        
        UniqueRoom();
        if (Input.GetKeyDown("space") && BoardManager.StartGame)
        {
            if (BoardManager.canshoot)
            {
                ShipShoot();
            }
        }
    }
    #endregion

    #region Public Methods
    public void UniqueRoom()
    {
        if (turnNo == 1)
        {
            playername.text = (PlayerNameInput.player1 + "'s turn");
            layerMask = 1 << 9;
        }
        else
        {
            playername.text = (PlayerNameInput.player2 + "'s turn");
            layerMask = 1 << 8;
        }
    }
    #endregion

    #region Photon Callback
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        localplayer = PhotonNetwork.LocalPlayer.UserId;
        localname = PhotonNetwork.NickName;
        Debug.Log(localname);
        Debug.Log(localplayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
        remoteplayer = newPlayer.UserId;
        remotename = newPlayer.NickName;
        Debug.Log(remotename);
        Debug.Log(remoteplayer); 
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("GameLauncher");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region GameRules
    public void BeginTurn()
    {
        //player 1 turn
        if (turnNo == 2)
        {
            turnNo = 1;
            nextplayer.text = ("Player 2's turn");
        }
        else
        {
            turnNo = 2;
            nextplayer.text = ("Player 1's turn");
        }
        StartPanel.SetActive(false);
    }

    void ShipShoot()
    {
        if (turnNo == 1)
        {
            playerPrefab = GameObject.Find(PlayerNameInput.player1);
            PlayerCamera = playerPrefab.GetComponentInChildren<Camera>();
        }
        if (turnNo == 2)
        {
            playerPrefab = GameObject.Find(PlayerNameInput.player2);
            PlayerCamera = playerPrefab.GetComponentInChildren<Camera>();
        }
        if (BoardManager.canshoot)
        {
            RaycastHit hit;
            if (turnNo == 1)
            {
                if (Physics.Raycast(PlayerCamera.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, layerMask))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.gameObject.tag == "Player2" && hitObject.gameObject.layer == 9)
                    {
                        strike = true;
                        Score.scoreA += 1;
                        StartPanel.SetActive(true);
                    }
                }
                else
                {
                    miss = true;
                    StartPanel.SetActive(true);
                }
            }       
            else
            {
                if (Physics.Raycast(PlayerCamera.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, layerMask))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.gameObject.tag == "Player1" && hitObject.gameObject.layer == 8)
                    {
                        strike = true;
                        Score.scoreB += 1;
                        StartPanel.SetActive(true);
                    }                    
                }
                else
                {
                    miss = true;
                    StartPanel.SetActive(true);
                }
            }
            GameObject Torpedo = Instantiate(torpedo);            
        }
        else
        {
            return;
        }        
    }
    #endregion
}


